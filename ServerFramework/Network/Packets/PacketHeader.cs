/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using System;

namespace ServerFramework.Network.Packets
{
	public class PacketHeader
	{
		#region Fields

		private int _length;
		private ushort _opcode;
		private byte _flags;

		#endregion

		#region Properties

		public int Length
		{
			get { return _length; }
			set { _length = value; }
		}

		public ushort Opcode
		{
			get { return _opcode; }
			set { _opcode = value; }
		}

		public byte Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		/// <summary>
		/// Is big packet (>64kb)
		/// </summary>
		public bool IsBigHeader
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.BigPacket); }
		}

		/// <summary>
		/// Is packet encoding unicode
		/// </summary>
		public bool IsUnicode
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.Unicode); }
		}

		/// <summary>
		/// Is packet loged in database
		/// </summary>
		public bool IsForLog
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.Log); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Network.Packets.PacketHeader"/> type.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		public PacketHeader(byte[] header)
		{
			Flags = header[0];

			Length = IsBigHeader
				? BitConverter.ToInt32(header, ServerConfig.PacketFlagsLength)
				: BitConverter.ToUInt16(header, ServerConfig.PacketFlagsLength);
			Opcode = IsBigHeader
				? BitConverter.ToUInt16(header
				,	ServerConfig.BigHeaderLength 
					- ServerConfig.OpcodeLength)
				: BitConverter.ToUInt16(header
				,	ServerConfig.HeaderLength 
					- ServerConfig.OpcodeLength);
		}

		#endregion
	}
}
