/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;
using System.Net;

namespace ServerFramework.Network.Session
{
	public sealed class Client
	{
		#region Fields

		private SocketExtended _socketExtended;
		private IServer _server;
		private IClient _clientToken;
		private CommandLevel _userLevel;

		#endregion

		#region Properties

		private IServer Server
		{
			get { return _server; }
			set { _server = value; }
		}

		public IClient Token
		{
			get { return _clientToken; }
			set { _clientToken = value; }
		}

		internal SocketExtended SocketExtended
		{
			get { return _socketExtended; }
			set { _socketExtended = value; }
		}

		private IPEndPoint EndPoint
		{
			get
			{
				IPEndPoint retVal = null;

				if (SocketExtended != null && SocketExtended.RemoteEndPoint != null)
					retVal = SocketExtended.RemoteEndPoint;

				return retVal;
			}
		}

		public string IP
		{
			get 
			{
				string retVal = String.Empty;

				if(EndPoint != null && EndPoint.Address != null)
					retVal =  EndPoint.Address.ToString();

				return retVal;
			}
		}

		public int Port
		{
			get 
			{
				int retVal = 0;

				if (EndPoint != null)
					retVal = EndPoint.Port;

				return retVal;
			}
		}

		public int SessionID
		{
			get 
			{
				int retVal = 0;

				if (SocketExtended != null)
					retVal = SocketExtended.ReceiverData.SessionId;

				return retVal;
			}
		}

		public bool IsConsole
		{
			get { return Token != null ? Token is ConsoleClient : false; }
		}

		public CommandLevel UserLevel
		{
			get { return _userLevel; }
			set { _userLevel = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates default instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.
		/// </summary>
		internal Client()
		{

		}

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.
		/// </summary>
		/// <param name="socketExtended">Instance of <see cref="ServerFramework.Network.Socket.SocketExtended"/> type.</param>
		internal Client(IServer server, SocketExtended socketExtended)
		{
			Server = server;
			SocketExtended = socketExtended;
		}

		#endregion

		#region Events

		public event EventHandler BeforePacketSend;

		#endregion

		#region Methods

		#region Send

		/// <summary>
		/// Sends packet to client.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		public void Send(ushort opcode, byte flags, int maxLength, Action<Packet> action)
		{
			SocketExtended.SendResetEvent.WaitOne();
			SocketData data = SocketExtended.SenderData;
			data.Packet.Alloc(maxLength);
			data.Packet.Stream.Seek(ServerConfig.BigHeaderLength);

			action(data.Packet);

			data.Finish(flags, opcode);
			data.Packet.SessionId = data.SessionId;

			if (BeforePacketSend != null)
				BeforePacketSend(data.Packet, new EventArgs());

			Server.Send(SocketExtended.Sender);
		}

		#endregion

		#endregion
	}
}
