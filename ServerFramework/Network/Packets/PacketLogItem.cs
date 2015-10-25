/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Enums;
using ServerFramework.Network.Session;

namespace ServerFramework.Network.Packets
{
	internal class PacketLogItem
	{
		#region Fields

		private Client _client;
		private PacketHeader _packetHeader;
		private byte[] _packetMessage;
		private PacketLogType _packetLogType;

		#endregion

		#region Properties

		internal Client Client
		{
			get { return _client; }
		}

		internal PacketHeader PacketHeader
		{
			get { return _packetHeader; }
		}

		internal byte[] PacketMessage
		{
			get { return _packetMessage; }
		}

		internal PacketLogType PacketLogType
		{
			get { return _packetLogType; }
		}

		#endregion

		#region Constructors

		public PacketLogItem(Client client, PacketHeader header, byte[] message, PacketLogType logtype)
		{
			_client = client;
			_packetHeader = header;
			_packetMessage = message;
			_packetLogType = logtype;
		}

		#endregion
	}
}
