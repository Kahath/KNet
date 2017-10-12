/*
 * Copyright © Kahath 2015
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
		private PacketLogTypes _packetLogType;

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

		internal PacketLogTypes PacketLogType
		{
			get { return _packetLogType; }
		}

		#endregion

		#region Constructors

		public PacketLogItem(Client client, PacketHeader header, byte[] message, PacketLogTypes logtype)
		{
			_client = client;
			_packetHeader = header;
			_packetMessage = message;
			_packetLogType = logtype;
		}

		#endregion
	}
}
