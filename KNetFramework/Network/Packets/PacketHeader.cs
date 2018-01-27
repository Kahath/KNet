/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Enums;
using System;
using UMemory.Unmanaged.Stream.Base;
using UMemory.Unmanaged.Stream.Core;

namespace KNetFramework.Network.Packets
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
		/// /// </summary>
		/// <param name="flags">Header flags.</param>
		/// <param name="length">Message length.</param>
		/// <param name="opcode">Header opcode.</param>
		public PacketHeader(byte flags, int length, ushort opcode)
		{
			Flags = flags;
			Length = length;
			Opcode = opcode;
		}

		/// <summary>
		/// Instantiates new <see cref="PacketHeader"/> type.
		/// </summary>
		public PacketHeader()
		{

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
			Flags = stream.ReadByte();
			Length = IsBigHeader ? stream.ReadInt32() : stream.ReadInt16();
			Opcode = stream.ReadUInt16();
		}

		#endregion

		#endregion
	}
}
