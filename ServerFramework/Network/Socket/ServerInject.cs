/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
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

		private bool _isRunning;

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

		public bool IsRunning
		{
			get { return _isRunning; }
		}

		#endregion

		#region Events

		public event ServerEventHandler ClosingClientSocket;
		public event ServerEventHandler Connect;

		#endregion

		#region Constructors

		ServerInject(SocketListenerSettings socketSettings)
		{
			SocketSettings = socketSettings;

			AcceptPool = new ObjectPool<SocketAsyncEventArgs>(SocketSettings.MaxAcceptOps);
			SendReceivePool = new ObjectPool<SocketExtended>(SocketSettings.NumberOfSaeaForRecSend);

			MaxConnections = new Semaphore(SocketSettings.MaxConnections, SocketSettings.MaxConnections);
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

			StartListen();
		}

		#endregion

		#region StartListen

		private void StartListen()
		{
			ListenSocket = new System.Net.Sockets.Socket(SocketSettings.LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				ListenSocket.Bind(SocketSettings.LocalEndPoint);
				ListenSocket.Listen(SocketSettings.Backlog);
			}
			catch (SocketException e)
			{
				Manager.LogMgr.Log(LogTypes.Error, e);
				Console.ReadLine();
				Environment.Exit(0);
			}

			Manager.LogMgr.Log(LogTypes.Normal, $"Starting listening on {SocketSettings.LocalEndPoint.Address}:{SocketSettings.LocalEndPoint.Port}");

			_isRunning = true;
			StartAccept();
		}

		#endregion

		#region StartAccept

		private void StartAccept()
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
				ProcessAccept(acceptEventArgs);
		}

		#endregion

		#region StartReceive

		private void StartReceive(SocketAsyncEventArgs e)
		{
			try
			{
				SocketData data = (SocketData)e.UserToken;
				e.SetBuffer(data.BufferOffset, SocketSettings.BufferSize);

				if (!e.AcceptSocket.ReceiveAsync(e))
					ProcessReceive(e);
			}
			catch (ArgumentOutOfRangeException) { CloseClientSocket(e); }
			catch (SocketException) { CloseClientSocket(e); }
			catch (ObjectDisposedException) { CloseClientSocket(e); }
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
					data.Packet.CopyTo(data.MessageBytesDoneCount, e.Buffer, data.BufferOffset, (uint)SocketSettings.BufferSize);
				}
				else
				{
					e.SetBuffer(data.BufferOffset, data.MessageBytesRemainingCount);
					data.Packet.CopyTo(data.MessageBytesDoneCount, e.Buffer, data.BufferOffset, (uint)data.MessageBytesRemainingCount);
				}

				if (!e.AcceptSocket.SendAsync(e))
					ProcessSend(e);
			}
			catch (ObjectDisposedException) { }
			catch (NullReferenceException) { }
			catch (ArgumentNullException) { }
		}

		#endregion

		#region ProcessAccept

		private void ProcessAccept(SocketAsyncEventArgs e)
		{
			StartAccept();

			if (e.SocketError == SocketError.Success)
			{
				SocketExtended socketExtended = SendReceivePool.Pop();

				Client c = new Client(this, socketExtended);
				int id = Manager.SessionMgr.AddClient(c);

				if (id != 0)
				{
					socketExtended.SessionID = id;
					socketExtended.AcceptSocket = e.AcceptSocket;

					try
					{
						Manager.LogMgr.Log(LogTypes.Normal, $"Session {socketExtended.ReceiverData.SessionID} ({socketExtended.Receiver.AcceptSocket.RemoteEndPoint}) connected");
					}
					catch (ObjectDisposedException)
					{

					}

					e.AcceptSocket = null;
					AcceptPool.Push(e);

					Connect?.Invoke(c, e);

					StartReceive(socketExtended.Receiver);
				}
				else
				{
					CloseClientSocket(e);
				}
			}
		}

		#endregion

		#region ProcessReceive

		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
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
						{
							data.Reset(data.MessageOffset + data.MessageBytesDoneThisOp);
						}
						else
						{
							data.Reset(data.BufferOffset);
						}
					}
					else if (data.IsHeaderReady)
					{
						data.MessageOffset = data.BufferOffset;
					}

					data.HeaderBytesDoneThisOp = 0;
					data.MessageBytesDoneThisOp = 0;
				}

				StartReceive(e);
			}
			else
			{
				CloseClientSocket(e);
			}
		}

		#endregion

		#region ProcessSend

		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				SocketData data = (SocketData)e.UserToken;

				data.MessageBytesRemainingCount -= e.BytesTransferred;

				if (data.MessageBytesRemainingCount == 0)
				{
					Client c = Manager.SessionMgr.GetClient(data.SessionID);

					if (c != null)
					{
						Manager.PacketLogMgr.Log(data.Packet);
						data.Reset(data.BufferOffset);

						c.SocketExtended.Signaler.SetGreen();
					}
				}
				else
				{
					data.MessageBytesDoneCount += e.BytesTransferred;
					StartSend(e);
				}
			}
			else
			{
				CloseClientSocket(e);
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

		private SocketAsyncEventArgs CreateNewSaeaForAccept()
		{
			SocketAsyncEventArgs acceptEventArgs = new SocketAsyncEventArgs();
			acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);

			return acceptEventArgs;
		}

		#endregion

		#region CreateNewSendRecPoolItem

		private SocketExtended CreateNewSendRecPoolItem()
		{
			SocketExtended retVal = new SocketExtended();
			Manager.BufferMgr.SetBuffer(retVal.Receiver);
			Manager.BufferMgr.SetBuffer(retVal.Sender);

			retVal.Receiver.UserToken = new SocketData(SocketSettings.BufferSize, retVal.Receiver.Offset, SocketSettings.HeaderLength, PacketLogTypes.CMSG);
			retVal.Receiver.Completed += new EventHandler<SocketAsyncEventArgs>(Receive_Completed);

			retVal.Sender.UserToken = new SocketData(SocketSettings.BufferSize, retVal.Sender.Offset, SocketSettings.HeaderLength, PacketLogTypes.SMSG);
			retVal.Sender.Completed += new EventHandler<SocketAsyncEventArgs>(Send_Completed);

			return retVal;
		}

		#endregion

		#region CloseClientSocket

		private void CloseClientSocket(SocketAsyncEventArgs e)
		{
			SocketData data = (SocketData)e.UserToken;

			Client c = Manager.SessionMgr.RemoveClient(data.SessionID);
			data.Reset(data.BufferOffset);

			if (c != null)
			{
				ClosingClientSocket?.Invoke(c, e);

				c.SocketExtended.Disconnect(SocketShutdown.Both);
				SendReceivePool.Push(c.SocketExtended);
				c = null;

				MaxConnections.Release();
			}
		}

		#endregion

		#region Exit

		public void Quit()
		{
			_isRunning = false;
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
			if (isDisposing)
			{
				ListenSocket.Dispose();
				MaxConnections.Dispose();
			}
		}

		#endregion

		#endregion

		#region +EventHandling

		#region Accept_Completed

		private void Accept_Completed(object sender, SocketAsyncEventArgs e)
		{
			ProcessAccept(e);
		}

		#endregion

		#region Receive_Completed

		private void Receive_Completed(object sender, SocketAsyncEventArgs e)
		{
			ProcessReceive(e);
		}

		#endregion

		#region Send_Completed

		private void Send_Completed(object sender, SocketAsyncEventArgs e)
		{
			ProcessSend(e);
		}

		#endregion

		#endregion
	}
}
