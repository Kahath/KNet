/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Network.Packets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework.Network.Socket
{
	internal sealed class SocketExtended : IDisposable
	{
		#region Fields

		private SocketAsyncEventArgs _sender;
		private SocketAsyncEventArgs _receiver;
		private AutoResetEvent _sendResetEvent;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates object with SocketAsyncEventArgs objects
		/// for sending and receiving data
		/// </summary>
		internal SocketExtended()
		{
			Sender = new SocketAsyncEventArgs();
			Receiver = new SocketAsyncEventArgs();
			_sendResetEvent = new AutoResetEvent(true);
		}

		#endregion

		#region Properties

		internal SocketAsyncEventArgs Sender
		{
			get { return _sender; }
			set { _sender = value; }
		}

		internal SocketAsyncEventArgs Receiver
		{
			get { return _receiver; }
			set { _receiver = value; }
		}

		internal SocketData ReceiverData
		{
			get { return ((SocketData)Receiver.UserToken); }
		}

		internal SocketData SenderData
		{
			get { return ((SocketData)Sender.UserToken); }
		}

		internal AutoResetEvent SendResetEvent
		{
			get { return _sendResetEvent; }
			set { _sendResetEvent = value; }
		}

		internal System.Net.Sockets.Socket AcceptSocket
		{
			set
			{
				Sender.AcceptSocket = value;
				Receiver.AcceptSocket = value;
			}
		}

		internal int SessionId
		{
			set
			{
				SenderData.SessionId = value;
				ReceiverData.SessionId = value;
			}
		}

		internal IPEndPoint RemoteEndPoint
		{
			get { return Receiver.AcceptSocket.RemoteEndPoint as IPEndPoint; }
		}

		#endregion

		#region Methods

		#region Close

		/// <summary>
		/// Closes both SocketAsyncEventArgs objects
		/// </summary>
		private void Close()
		{
			this.Sender.AcceptSocket.Close();
		}

		#endregion

		#region Disconnect

		/// <summary>
		/// Shutdown both SocketAsyncEventArgs objects
		/// </summary>
		/// <param name="how"></param>
		internal void Disconnect(SocketShutdown how)
		{
			try
			{
				this.Sender.AcceptSocket.Shutdown(how);
				this.SendResetEvent.Set();
				this.Close();
			}
			catch (SocketException) { }
		}

		#endregion

		#region Dispose

		public void Dispose()
		{
			_sendResetEvent.Dispose();
		}

		#endregion

		#endregion
	}
}
