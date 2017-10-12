/*
 * Copyright © Kahath 2015
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
				return SocketExtended?.RemoteEndPoint;
			}
		}

		public string IP
		{
			get
			{
				return EndPoint?.Address?.ToString() ?? String.Empty;
			}
		}

		public int Port
		{
			get
			{
				return EndPoint?.Port ?? 0;
			}
		}

		public int SessionID
		{
			get
			{
				return SocketExtended?.ReceiverData?.SessionID ?? 0;
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
		/// Creates default instance of <see cref="Client"/> type.
		/// </summary>
		internal Client()
		{

		}

		/// <summary>
		/// Creates instance of <see cref="Client"/> type.
		/// </summary>
		/// <param name="server">Instance of <see cref="IServer"/> type.</param>
		/// <param name="socketExtended">Instance of <see cref="Socket.SocketExtended"/> type.</param>
		internal Client(IServer server, SocketExtended socketExtended)
		{
			_server = server;
			_socketExtended = socketExtended;
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
		/// <param name="opcode">Packet opcode.</param>
		/// <param name="maxLength">Estimate max length of packet underlying stream.</param>
		/// <param name="action">Packet action.</param>
		public void Send(ushort opcode, int maxLength, Action<Packet> action)
		{
			Send(opcode, 0, maxLength, action);
		}

		/// <summary>
		/// Sends packet to client.
		/// </summary>
		/// <param name="opcode">Packet opcode.</param>
		/// <param name="flags">Packet flags.</param>
		/// <param name="maxLength">Estimate max length of packet underlying stream.</param>
		/// <param name="action">Packet action.</param>
		public async void Send(ushort opcode, byte flags, int maxLength, Action<Packet> action)
		{
			await SocketExtended.Signaler.WaitGreen();

			SocketData data = SocketExtended.SenderData;
			data.Packet.Alloc(maxLength);
			data.Packet.Stream.Seek(ServerConfig.BigHeaderLength);

			action(data.Packet);
			data.Finish(flags, opcode);

			BeforePacketSend?.Invoke(data.Packet, new EventArgs());

			Server.Send(SocketExtended.Sender);
		}

		#endregion

		#endregion
	}
}
