/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Enums;
using System;
using UMemory.Unmanaged.Stream.Base;
using UMemory.Unmanaged.Stream.Core;

namespace ServerFramework.Network.Packets
{
	public class PacketHeader : IUMemoryRead, IUMemoryWrite
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
			get { return Convert.ToBoolean(Flags & (byte)PacketFlags.BigPacket); }
		}

		/// <summary>
		/// Is packet loged in database
		/// </summary>
		public bool IsForLog
		{
			get { return Convert.ToBoolean(Flags & (byte)PacketFlags.Log); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="PacketHeader"/> type.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		public PacketHeader(byte flags, int length, ushort opcode)
		{
			Flags = flags;
			Length = length;
			Opcode = opcode;
		}

		#endregion

		#region Methods

		#region Reset

		/// <summary>
		/// Resets values for packet header.
		/// </summary>
		internal void Reset()
		{
			_flags = 0;
			_length = 0;
			_opcode = 0;
		}

		#endregion

		#region Write

		/// <summary>
		/// Writes data to underlying stream.
		/// </summary>
		/// <param name="stream">Underlying <see cref="UMemoryStream"/></param>
		public void Write(UMemoryStream stream)
		{
			stream.Write(Flags);
			stream.Write(IsBigHeader ? Length : (ushort)Length);
			stream.Write(Opcode);
		}

		#endregion

		#region Read

		/// <summary>
		/// Reads data from underlying stream.
		/// </summary>
		/// <param name="stream">Underlying <see cref="UMemoryStream"/></param>
		public void Read(UMemoryStream stream)
		{
			Flags = stream.Read<byte>();
			Length = IsBigHeader ? stream.Read<int>() : stream.Read<ushort>();
			Opcode = stream.Read<ushort>();
		}

		#endregion

		#endregion
	}
}
