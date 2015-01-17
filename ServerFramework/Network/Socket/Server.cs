/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using ServerFramework.Configuration;
using ServerFramework.Constants.Entities.Console.Misc;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Handlers;
using ServerFramework.Network.Packets;
using ServerFramework.Singleton;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework.Network.Socket
{
    public class Server : SingletonBase<Server>, IDisposable
    {
        #region Fields

        System.Net.Sockets.Socket listenSocket;

        Semaphore maxConnectionsEnforcer;
        SocketListenerSettings socketSettings;

        ObjectPool<SocketAsyncEventArgs> AcceptPool;
        ObjectPool<Saea> SendReceivePool;

        HeaderHandler headerHandler;
        MessageHandler messageHandler;

        #endregion 

        #region Events

        public event ServerEventHandler OnCloseClientSocket;
        public event ServerEventHandler OnConnect;

        #endregion

        #region Constructors

        Server(SocketListenerSettings socketSettings)
        {
            this.socketSettings = socketSettings;

            this.AcceptPool = new
                ObjectPool<SocketAsyncEventArgs>(this.socketSettings.MaxAcceptOps);
            this.SendReceivePool = new
                ObjectPool<Saea>(this.socketSettings.NumberOfSaeaForRecSend);

            this.maxConnectionsEnforcer = new Semaphore(
                this.socketSettings.MaxConnections,
                this.socketSettings.MaxConnections);

            headerHandler = new HeaderHandler();
            messageHandler = new MessageHandler();
        }

        #endregion

        #region Methods

        #region Init

        internal override void Init()
        {
            for (int i = 0; i < socketSettings.MaxAcceptOps; i++)
                this.AcceptPool.Push(
                    CreateNewSaeaForAccept(AcceptPool));

            Saea SendRecPoolItem;
            UserToken token;

            for (int i = 0; i < socketSettings.NumberOfSaeaForRecSend; i++)
            {
                SendRecPoolItem = new Saea();
                SendRecPoolItem.Receiver = new SocketAsyncEventArgs();
                SendRecPoolItem.Sender = new SocketAsyncEventArgs();
                Manager.BufferMgr.SetBuffer(SendRecPoolItem.Receiver);
                Manager.BufferMgr.SetBuffer(SendRecPoolItem.Sender);

                token = new UserToken(socketSettings.BufferSize,
                    SendRecPoolItem.Receiver.Offset,
                    socketSettings.ReceivePrefixLength);
                token.StartReceive();
                SendRecPoolItem.Receiver.UserToken = token;
                SendRecPoolItem.Receiver.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(receive_completed);

                token = new UserToken(socketSettings.BufferSize, SendRecPoolItem.Sender.Offset,
                    socketSettings.ReceivePrefixLength);
                SendRecPoolItem.Sender.UserToken = token;
                SendRecPoolItem.Sender.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(send_completed);

                this.SendReceivePool.Push(SendRecPoolItem);
            }

            startListen();

            base.Init();
        }

        #endregion

        #region StartListen

        private void startListen()
        {
            listenSocket = new System.Net.Sockets.Socket(this.socketSettings.LocalEndPoint.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(this.socketSettings.LocalEndPoint);

                listenSocket.Listen(this.socketSettings.Backlog);
            }
            catch (SocketException e)
            {
                LogManager.Log(LogType.Error, "{0}", e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }

            LogManager.Log(LogType.Normal, "Starting listening on {0}:{1}",
                this.socketSettings.LocalEndPoint.Address,
                this.socketSettings.LocalEndPoint.Port);

            startAccept();
        }

        #endregion

        #region StartAccept

        private void startAccept()
        {
            LogManager.Log(LogType.Debug, "Start Accepting connection");
            SocketAsyncEventArgs acceptEventArgs;

            if (this.AcceptPool.Count > 1)
            {
                try
                {
                    acceptEventArgs = this.AcceptPool.Pop();
                }
                catch
                {
                    acceptEventArgs = CreateNewSaeaForAccept(AcceptPool);
                }
            }
            else
            {
                acceptEventArgs = CreateNewSaeaForAccept(AcceptPool);
            }

            this.maxConnectionsEnforcer.WaitOne();

            if (!listenSocket.AcceptAsync(acceptEventArgs))
                processAccept(acceptEventArgs);
        }

        #endregion

        #region StartReceive

        private void startReceive(SocketAsyncEventArgs e)
        {
            try
            {
                UserToken token = (UserToken)e.UserToken;
                e.SetBuffer(token.BufferOffset, this.socketSettings.BufferSize);

                if (!e.AcceptSocket.ReceiveAsync(e))
                    processReceive(e);
            }
            catch (Exception)
            {
                closeClientSocket(e);
            }
        }

        #endregion

        #region StartSend

        private void StartSend(SocketAsyncEventArgs e)
        {
            try
            {
                UserToken token = (UserToken)e.UserToken;

                if (token.MessageBytesRemainingCount > this.socketSettings.BufferSize)
                {
                    e.SetBuffer(token.BufferOffset, this.socketSettings.BufferSize);

                    Buffer.BlockCopy(token.Packet.Message, token.MessageBytesDoneCount,
                        e.Buffer, token.BufferOffset, this.socketSettings.BufferSize);
                }
                else
                {
                    e.SetBuffer(token.BufferOffset, token.MessageBytesRemainingCount);

                    Buffer.BlockCopy(token.Packet.Message, token.MessageBytesDoneCount,
                        e.Buffer, token.BufferOffset, token.MessageBytesRemainingCount);
                }

                if (!e.AcceptSocket.SendAsync(e))
                    processSend(e);
            }
            catch (ObjectDisposedException) { }
            catch (NullReferenceException) { }
            catch (ArgumentNullException) { }
        }

        #endregion

        #region ProcessAccept

        private void processAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                loopToStartAccept();
            }

            loopToStartAccept();

            Saea saea = SendReceivePool.Pop();

            Client c = new Client(saea);
            int id = Manager.SessionMgr.AddClient(c);

            saea.AssignId(id);
            saea.AcceptSocket = e.AcceptSocket;

            try
            {
                LogManager.Log(LogType.Normal, "Session {0} ({1}) connected",
                    ((UserToken)saea.Receiver.UserToken).SessionId,
                    saea.Receiver.AcceptSocket.RemoteEndPoint.ToString());
            }
            catch(ObjectDisposedException)
            {

            }

            e.AcceptSocket = null;
            this.AcceptPool.Push(e);

            if (OnConnect != null)
                OnConnect(c, e);

            startReceive(saea.Receiver);
        }

        #endregion

        #region ProcessReceive

        private void processReceive(SocketAsyncEventArgs e)
        {
            UserToken token = (UserToken)e.UserToken;
            if (e.SocketError != SocketError.Success)
            {
                closeClientSocket(e);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                closeClientSocket(e);
                return;
            }

            int remainingBytes = e.BytesTransferred;

            while (remainingBytes > 0)
            {
                if (!token.HeaderReady)
                    remainingBytes = headerHandler.HandleHeader(e, token, remainingBytes);

                if (token.HeaderReady)
                    remainingBytes = messageHandler.HandleMessage(e, token, remainingBytes);

                if (token.PacketReady)
                {
                    Manager.PacketLogMgr.Log(token.Packet);

                    Manager.PacketMgr.InvokeHandler(token.Packet);

                    if (remainingBytes > 0)
                        token.Reset(token.MessageOffset + token.MessageLength + 4);
                    else
                        token.Reset(token.PermanentMessageOffset);
                }
                else
                {
                    if (token.HeaderReady)
                    {
                        token.MessageOffset = token.BufferOffset;
                        token.HeaderBytesDoneCount = 0;
                    }
                }
            }

            startReceive(e);
        }

        #endregion

        #region ProcessSend

        private void processSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                closeClientSocket(e);
                return;
            }

            UserToken token = (UserToken)e.UserToken;

            token.MessageBytesRemainingCount -= e.BytesTransferred;

            if (token.MessageBytesRemainingCount == 0)
            {
                Client c = Manager.SessionMgr.GetClientBySessionId(token.SessionId);

                if (c == null)
                    return;

                Manager.PacketLogMgr.Log(token.Packet);
                token.Reset(token.PermanentMessageOffset);

                c.Saea.SendResetEvent.Set();
            }
            else
            {
                token.MessageBytesDoneCount += e.BytesTransferred;
                StartSend(e);
            }
        }

        #endregion

        #region Send

        internal void Send(SocketAsyncEventArgs e)
        {
            StartSend(e);
        }

        #endregion

        #region CreateNewSaeaForAccept

        private SocketAsyncEventArgs CreateNewSaeaForAccept(ObjectPool<SocketAsyncEventArgs> pool)
        {
            SocketAsyncEventArgs acceptEventArgs = new SocketAsyncEventArgs();
            acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(accept_completed);

            return acceptEventArgs;
        }

        #endregion

        #region LoopToStartAccept

        private void loopToStartAccept()
        {
            startAccept();
        }

        #endregion

        #region CloseClientSocket

        private void closeClientSocket(SocketAsyncEventArgs e)
        {
            Client c = Manager.SessionMgr.RemoveClient(((UserToken)e.UserToken).SessionId);

            if (c == null)
                return;

            if (OnCloseClientSocket != null)
                OnCloseClientSocket(c, e);

            LogManager.Log(LogType.Normal, "Session {0} quit", ((UserToken)e.UserToken).SessionId);

            c.Saea.Shutdown(SocketShutdown.Both);
            c.Saea.Close();
            this.SendReceivePool.Push(c.Saea);
            c = null;
            this.maxConnectionsEnforcer.Release();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region +EventHandling

        #region accept_completed

        private void accept_completed(object sender, SocketAsyncEventArgs e)
        {
            processAccept(e);
        }

        #endregion

        #region receive_completed

        private void receive_completed(object sender, SocketAsyncEventArgs e)
        {
            processReceive(e);
        }

        #endregion

        #region send_completed

        private void send_completed(object sender, SocketAsyncEventArgs e)
        {
            processSend(e);
        }

        #endregion

        #endregion
    }
}
