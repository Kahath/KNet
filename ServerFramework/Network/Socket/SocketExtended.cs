/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Async.Semaphore;
using ServerFramework.Network.Packets;
using System.Net;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	internal sealed class SocketExtended
	{
		#region Fields

		private SocketAsyncEventArgs _sender;
		private SocketAsyncEventArgs _receiver;
		private Signaler _signaler;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates object with SocketAsyncEventArgs objects
		/// for sending and receiving data
		/// </summary>
		internal SocketExtended()
		{
			_sender = new SocketAsyncEventArgs();
			_receiver = new SocketAsyncEventArgs();
			_signaler = new Signaler();
		}

		#endregion

		#region Properties

		internal Signaler Signaler
		{
			get { return _signaler; }
		}

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

		internal System.Net.Sockets.Socket AcceptSocket
		{
			set
			{
				Sender.AcceptSocket = value;
				Receiver.AcceptSocket = value;
			}
		}

		internal int SessionID
		{
			set
			{
				SenderData.SessionID = value;
				ReceiverData.SessionID = value;
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
			Sender.AcceptSocket.Close();
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
				Sender.AcceptSocket.Shutdown(how);
				Signaler.SetGreen();
				Close();
			}
			catch (SocketException) { }
		}

		#endregion

		#endregion
	}
}
