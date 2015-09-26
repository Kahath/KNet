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

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Extensions;
using ServerFramework.Helpers;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Session;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework.Network.Socket
{
	public class ServerInject : IServer
	{
		#region Fields

		private System.Net.Sockets.Socket _listenSocket;

		private Semaphore _maxConnections;
		private SocketListenerSettings _socketSettings;

		private ObjectPool<SocketAsyncEventArgs> _acceptPool;
		private ObjectPool<SocketExtended> _sendReceivePool;

		#endregion

		#region Properties

		public System.Net.Sockets.Socket ListenSocket
		{
			get { return _listenSocket; }
			set { _listenSocket = value; }
		}

		private Semaphore MaxConnections
		{
			get { return _maxConnections; }
			set { _maxConnections = value; }
		}

		private SocketListenerSettings SocketSettings
		{
			get { return _socketSettings; }
			set { _socketSettings = value; }
		}

		private ObjectPool<SocketAsyncEventArgs> AcceptPool
		{
			get { return _acceptPool; }
			set { _acceptPool = value; }
		}

		private ObjectPool<SocketExtended> SendReceivePool
		{
			get { return _sendReceivePool; }
			set { _sendReceivePool = value; }
		}

		#endregion

		#region Events

		public event ServerEventHandler CloseClientSocket;
		public event ServerEventHandler Connect;

		#endregion

		#region Constructors

		ServerInject(SocketListenerSettings socketSettings)
		{
			SocketSettings = socketSettings;

			AcceptPool = new
				ObjectPool<SocketAsyncEventArgs>(SocketSettings.MaxAcceptOps);
			SendReceivePool = new
				ObjectPool<SocketExtended>(SocketSettings.NumberOfSaeaForRecSend);

			MaxConnections = new Semaphore(SocketSettings.MaxConnections,
				SocketSettings.MaxConnections);
		}

		#endregion

		#region Methods

		#region Init

		public void Init()
		{
			for (int i = 0; i < SocketSettings.MaxAcceptOps; i++)
				AcceptPool.Push(CreateNewSaeaForAccept());

			for (int i = 0; i < SocketSettings.NumberOfSaeaForRecSend; i++)
				SendReceivePool.Push(CreateNewSendRecPoolItem());

			startListen();
		}

		#endregion

		#region StartListen

		private void startListen()
		{
			ListenSocket = new System.Net.Sockets.Socket(SocketSettings.LocalEndPoint.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

			try
			{
				ListenSocket.Bind(SocketSettings.LocalEndPoint);

				ListenSocket.Listen(SocketSettings.Backlog);
			}
			catch (SocketException e)
			{
				Manager.LogMgr.Log(LogType.Error, $"{e.Message}", e.Message);
				Console.ReadLine();
				Environment.Exit(0);
			}

			Manager.LogMgr.Log(LogType.Normal, "Starting listening on "
				+ $"{SocketSettings.LocalEndPoint.Address}:{SocketSettings.LocalEndPoint.Port}");

			startAccept();
		}

		#endregion

		#region StartAccept

		private void startAccept()
		{
			SocketAsyncEventArgs acceptEventArgs;

			if (AcceptPool.Count > 1)
			{
				try
				{
					acceptEventArgs = AcceptPool.Pop();
				}
				catch
				{
					acceptEventArgs = CreateNewSaeaForAccept();
				}
			}
			else
			{
				acceptEventArgs = CreateNewSaeaForAccept();
			}

			MaxConnections.WaitOne();

			if (!ListenSocket.AcceptAsync(acceptEventArgs))
				processAccept(acceptEventArgs);
		}

		#endregion

		#region StartReceive

		private void startReceive(SocketAsyncEventArgs e)
		{
			try
			{
				SocketData data = (SocketData)e.UserToken;
				e.SetBuffer(data.BufferOffset, SocketSettings.BufferSize);

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
				SocketData data = (SocketData)e.UserToken;

				if (data.MessageBytesRemainingCount > SocketSettings.BufferSize)
				{
					e.SetBuffer(data.BufferOffset, SocketSettings.BufferSize);

					Buffer.BlockCopy(data.Packet.Message, data.MessageBytesDoneCount,
						e.Buffer, data.BufferOffset, SocketSettings.BufferSize);
				}
				else
				{
					e.SetBuffer(data.BufferOffset, data.MessageBytesRemainingCount);

					Buffer.BlockCopy(data.Packet.Message, data.MessageBytesDoneCount,
						e.Buffer, data.BufferOffset, data.MessageBytesRemainingCount);
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

				Client c = new Client(this, socketExtended);
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
					Manager.LogMgr.Log
						(
							LogType.Normal
						,	$"Session {socketExtended.ReceiverData.SessionId} " 
						+	$"({socketExtended.Receiver.AcceptSocket.RemoteEndPoint}) connected"
						);
				}
				catch (ObjectDisposedException)
				{

				}

				e.AcceptSocket = null;
				AcceptPool.Push(e);

				if (Connect != null)
					Connect(c, e);

				startReceive(socketExtended.Receiver);
			}
		}

		#endregion

		#region ProcessReceive

		private void processReceive(SocketAsyncEventArgs e)
		{
			if(e.SocketError == SocketError.Success && e.BytesTransferred > 0)
			{
				SocketData data = (SocketData)e.UserToken;
				int remainingBytes = e.BytesTransferred;

				while (remainingBytes > 0)
				{
					if (!data.IsHeaderReady)
					{
						remainingBytes = data.HandleHeader(e, remainingBytes);

						if (data.IsHeaderReady)
							remainingBytes = data.HandleMessage(e, remainingBytes);
					}
					else
					{
						remainingBytes = data.HandleMessage(e, remainingBytes);
					}

					if (data.IsPacketReady)
					{
						Manager.PacketLogMgr.Log(data.Packet);
						Manager.PacketMgr.InvokeHandler(data.Packet);

						if (remainingBytes > 0)
							data.Reset(data.MessageOffset + data.MessageBytesDoneThisOp);
						else
							data.Reset(data.BufferOffset);
					}
					else
					{
						if (data.IsHeaderReady)
						{
							data.MessageOffset = data.BufferOffset;
						}
					}

					data.HeaderBytesDoneThisOp = 0;
					data.MessageBytesDoneThisOp = 0;
				}

				startReceive(e);
			}
			else
				closeClientSocket(e);
		}

		#endregion

		#region ProcessSend

		private void processSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				SocketData data = (SocketData)e.UserToken;

				data.MessageBytesRemainingCount -= e.BytesTransferred;

				if (data.MessageBytesRemainingCount == 0)
				{
					Client c = Manager.SessionMgr.GetClientBySession(data.SessionId);

					if (c != null)
					{
						Manager.PacketLogMgr.Log(data.Packet);
						data.Reset(data.BufferOffset);

						c.SocketExtended.SendResetEvent.Set();
					}
				}
				else
				{
					data.MessageBytesDoneCount += e.BytesTransferred;
					StartSend(e);
				}
			}
			else
				closeClientSocket(e);
		}

		#endregion

		#region Send

		public void Send(SocketAsyncEventArgs e)
		{
			StartSend(e);
		}

		#endregion

		#region CreateNewSaeaForAccept

		private SocketAsyncEventArgs CreateNewSaeaForAccept()
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
			SocketData data = null;

			retVal = new SocketExtended();

			Manager.BufferMgr.SetBuffer(retVal.Receiver);
			Manager.BufferMgr.SetBuffer(retVal.Sender);

			data = new SocketData(SocketSettings.BufferSize,
				retVal.Receiver.Offset,
				SocketSettings.HeaderLength);

			retVal.Receiver.UserToken = data;
			retVal.Receiver.Completed +=
				new EventHandler<SocketAsyncEventArgs>(receive_completed);

			data = new SocketData(SocketSettings.BufferSize, retVal.Sender.Offset,
				SocketSettings.HeaderLength);

			retVal.Sender.UserToken = data;
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
			SocketData data = (SocketData)e.UserToken;

			Client c = Manager.SessionMgr.RemoveClient(data.SessionId);
			data.Reset(data.BufferOffset);

			if (c != null)
			{
				if (CloseClientSocket != null)
					CloseClientSocket(c, e);

				Manager.LogMgr.Log(LogType.Normal, $"Session {((SocketData)e.UserToken).SessionId} quit");

				c.SocketExtended.Disconnect(SocketShutdown.Both);

				SendReceivePool.Push(c.SocketExtended);
				c = null;

				MaxConnections.Release();
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
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if(isDisposing)
			{
				ListenSocket.Dispose();
				MaxConnections.Dispose();
			}
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
