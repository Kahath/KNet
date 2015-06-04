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
using ServerFramework.Extensions;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework.Network.Socket
{
	public class ServerInject : IServer
	{
		#region Fields

		System.Net.Sockets.Socket listenSocket;

		Semaphore maxConnectionsEnforcer;
		SocketListenerSettings socketSettings;

		ObjectPool<SocketAsyncEventArgs> AcceptPool;
		ObjectPool<SocketExtended> SendReceivePool;

		#endregion

		#region Events

		public event ServerEventHandler OnCloseClientSocket;
		public event ServerEventHandler OnConnect;

		#endregion

		#region Constructors

		ServerInject(SocketListenerSettings socketSettings)
		{
			this.socketSettings = socketSettings;

			this.AcceptPool = new
				ObjectPool<SocketAsyncEventArgs>(this.socketSettings.MaxAcceptOps);
			this.SendReceivePool = new
				ObjectPool<SocketExtended>(this.socketSettings.NumberOfSaeaForRecSend);

			this.maxConnectionsEnforcer = new Semaphore(
				this.socketSettings.MaxConnections,
				this.socketSettings.MaxConnections);
		}

		#endregion

		#region Methods

		#region Init

		public void Init()
		{
			for (int i = 0; i < socketSettings.MaxAcceptOps; i++)
				this.AcceptPool.Push(
					CreateNewSaeaForAccept(AcceptPool));

			for (int i = 0; i < socketSettings.NumberOfSaeaForRecSend; i++)
				this.SendReceivePool.Push(CreateNewSendRecPoolItem());

			startListen();
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
				Manager.LogMgr.Log(LogType.Error, "{0}", e.Message);
				Console.ReadLine();
				Environment.Exit(0);
			}

			Manager.LogMgr.Log(LogType.Normal, "Starting listening on {0}:{1}",
				this.socketSettings.LocalEndPoint.Address,
				this.socketSettings.LocalEndPoint.Port);

			startAccept();
		}

		#endregion

		#region StartAccept

		private void startAccept()
		{
			Manager.LogMgr.Log(LogType.Info, "Start Accepting connection");
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
			catch (ArgumentOutOfRangeException) { closeClientSocket(e); }
			catch (SocketException) { closeClientSocket(e); }
			catch (ObjectDisposedException) { closeClientSocket(e); }
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
			loopToStartAccept();

			if (e.SocketError == SocketError.Success)
			{
				SocketExtended socketExtended = SendReceivePool.Pop();

				Client c = new Client(socketExtended);
				int id = Manager.SessionMgr.AddClient(c);

				if (id == 0)
				{
					closeClientSocket(e);
					return;
				}

				socketExtended.SessionId = id;
				socketExtended.AcceptSocket = e.AcceptSocket;

				try
				{
					Manager.LogMgr.Log(LogType.Normal, "Session {0} ({1}) connected",
						((UserToken)socketExtended.Receiver.UserToken).SessionId,
						socketExtended.Receiver.AcceptSocket.RemoteEndPoint);
				}
				catch (ObjectDisposedException)
				{

				}

				e.AcceptSocket = null;
				this.AcceptPool.Push(e);

				if (OnConnect != null)
					OnConnect(c, e);

				startReceive(socketExtended.Receiver);
			}
		}

		#endregion

		#region ProcessReceive

		private void processReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
			{
				closeClientSocket(e);
				return;
			}

			UserToken token = (UserToken)e.UserToken;
			int remainingBytes = e.BytesTransferred;

			while (remainingBytes > 0)
			{
				if (!token.HeaderReady)
				{
					remainingBytes = token.HandleHeader(e, remainingBytes);

					if (remainingBytes > 0 && token.HeaderReady)
						remainingBytes = token.HandleMessage(e, remainingBytes);
				}
				else
				{
					remainingBytes = token.HandleMessage(e, remainingBytes);
				}

				if (token.PacketReady)
				{
					Manager.PacketLogMgr.Log(token.Packet);
					Manager.PacketMgr.InvokeHandler(token.Packet);

					if (remainingBytes > 0)
						token.Reset(token.MessageOffset + token.MessageLength + socketSettings.HeaderLength);
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

				c.SocketExtended.SendResetEvent.Set();
			}
			else
			{
				token.MessageBytesDoneCount += e.BytesTransferred;
				StartSend(e);
			}
		}

		#endregion

		#region Send

		public void Send(SocketAsyncEventArgs e)
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

		#region CreateNewSendRecPoolItem

		private SocketExtended CreateNewSendRecPoolItem()
		{
			SocketExtended retVal = null;
			UserToken token = null;

			retVal = new SocketExtended();

			Manager.BufferMgr.SetBuffer(retVal.Receiver);
			Manager.BufferMgr.SetBuffer(retVal.Sender);

			token = new UserToken(socketSettings.BufferSize,
				retVal.Receiver.Offset,
				socketSettings.HeaderLength);
			token.StartReceive();

			retVal.Receiver.UserToken = token;
			retVal.Receiver.Completed +=
				new EventHandler<SocketAsyncEventArgs>(receive_completed);

			token = new UserToken(socketSettings.BufferSize, retVal.Sender.Offset,
				socketSettings.HeaderLength);

			retVal.Sender.UserToken = token;
			retVal.Sender.Completed +=
				new EventHandler<SocketAsyncEventArgs>(send_completed);

			return retVal;
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

			if (c != null)
			{
				if (OnCloseClientSocket != null)
					OnCloseClientSocket(c, e);

				Manager.LogMgr.Log(LogType.Normal, "Session {0} quit", ((UserToken)e.UserToken).SessionId);

				c.SocketExtended.Disconnect(SocketShutdown.Both);

				this.SendReceivePool.Push(c.SocketExtended);
				c = null;

				this.maxConnectionsEnforcer.Release();
			}
		}

		#endregion

		#region Exit

		internal void Exit()
		{
			Environment.Exit(0);
		}

		#endregion

		#region Dispose

		public void Dispose()
		{
			listenSocket.Dispose();
			maxConnectionsEnforcer.Dispose();
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
