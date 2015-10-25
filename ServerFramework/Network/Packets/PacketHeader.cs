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
		/// Is packet loged in database
		/// </summary>
		public bool IsForLog
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlag.Log); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="PacketHeader"/> type.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		public PacketHeader(byte flags, int length, ushort opcode)
		{
			Flags = flags;
			Length = length;
			Opcode = opcode;
		}

		#endregion
	}
}
