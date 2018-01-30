/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Configuration.Helpers;
using KNetFramework.Enums;
using System;
using UMemory.Unmanaged.Stream.Core;

namespace KNetFramework.Network.Packets
{
	internal class PacketStream : UMemoryStream
	{
		#region Fields

		private byte _bitPosition = 0;
		private byte _value;

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="PacketStream"/> type.
		/// </summary>
		/// <param name="maxLength">Length of array to allocate on underlying stream.</param>
		public PacketStream(int maxLength)
			: base(maxLength, KNetConfig.Endianness)
		{
		}

		#endregion

		#region Methods

		#region BitPack

		#region Read

		/// <summary>
		/// Reads one bit from underlying stream.
		/// </summary>
		/// <returns>Bit as boolean.</returns>
		internal bool ReadBit()
		{
			if (_bitPosition == 0)
			{
				_value = ReadUInt8();
				_bitPosition = 8;
			}

			bool retVal = (_value >> 7) == 1;

			--_bitPosition;
			_value <<= 1;

			return retVal;
		}

		/// <summary>
		/// Reads number of bits from underlying stream
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="count">Number of bits.</param>
		/// <returns>Value of generic type.</returns>
		internal T ReadBits<T>(int count)
			where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			int retVal = 0;

			for (int i = count - 1; i >= 0; --i)
				retVal = ReadBit() ? (1 << i) | retVal : retVal;

			return (T)Convert.ChangeType(retVal, typeof(T));
		}

		#endregion

		#region Write

		/// <summary>
		/// Writes one bit to underlying stream.
		/// </summary>
		/// <param name="value">Value.</param>
		internal void WriteBit(bool value)
		{
			++_bitPosition;

			if (value)
				_value |= (byte)(1 << (8 - _bitPosition));

			if (_bitPosition == 8)
			{
				WriteUInt8(_value);
				_bitPosition = 0;
				_value = 0;
			}
		}

		/// <summary>
		/// Writes number of bits to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="count">Number of bits.</param>
		internal void WriteBits<T>(T value, int count)
			where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			for (int i = count - 1; i >= 0; --i)
				WriteBit(((Convert.ToInt32(value) >> i) & 1) == 1);
		}

		/// <summary>
		/// Writes number of bits with start index to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="count">Number of bits.</param>
		internal void WriteBits<T>(T value, int startIndex, int count)
			where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			for (int i = startIndex + count - 1; i >= startIndex; --i)
				WriteBit(((Convert.ToInt32(value) >> i) & 1) == 1);
		}

		#endregion

		#endregion

		#region End

		/// <summary>
		/// Finishes writing to underlying stream.
		/// </summary>
		/// <param name="header">Instance of <see cref="PacketHeader"/> type.</param>
		/// <param name="flags">Packet flags.</param>
		/// <param name="opcode">Packet opcode.</param>
		/// <returns></returns>
		internal int End(PacketHeader header, byte flags, ushort opcode)
		{
			Flush(BitPackFlushType.Write);

			int messageLength = Position - KNetConfig.BigHeaderLength;
			bool isBigHeader = messageLength > UInt16.MaxValue;

			flags = SetupFlag(flags, PacketFlags.BigPacket, isBigHeader);

			int headerLength = isBigHeader ? KNetConfig.BigHeaderLength : KNetConfig.HeaderLength;
			int packetPosition = KNetConfig.BigHeaderLength - headerLength;

			Seek(packetPosition);

			header.Flags = flags;
			header.Length = messageLength;
			header.Opcode = opcode;

			Write(header);
			Adjust(packetPosition);

			return messageLength + headerLength;
		}

		#endregion

		#region Flush

		/// <summary>
		/// Writes remaining bits to underlying stream and resets position.
		/// </summary>
		internal void FlushWrite()
		{
			if (_bitPosition != 0)
			{
				WriteUInt8(_value);

				_bitPosition = 0;
				_value = 0;
			}
		}

		/// <summary>
		/// Resets bit position on underlying stream bitpack.
		/// </summary>
		private void FlushRead()
		{
			if (_bitPosition != 0)
			{
				_bitPosition = 0;
				_value = 0;
			}
		}

		/// <summary>
		/// Resets bit position on underlying stream bitpack.
		/// </summary>
		/// <param name="flushType"><see cref="BitPackFlushType"/> type</param>
		internal void Flush(BitPackFlushType flushType)
		{
			if (flushType == BitPackFlushType.Read)
				FlushRead();
			else
				FlushWrite();
		}

		#endregion

		#region ResetPosition

		/// <summary>
		/// Resets position of underlying stream.
		/// </summary>
		internal void ResetPosition()
		{
			Seek(0);
		}

		#endregion

		#region SetupFlag

		/// <summary>
		/// Sets or removes flag.
		/// </summary>
		/// <param name="flags">Flags to setup.</param>
		/// <param name="flag">Flag to set or remove.</param>
		/// <param name="isSet">Set or remove.</param>
		/// <returns>New flags value.</returns>
		public byte SetupFlag(byte flags, PacketFlags flag, bool isSet)
		{
			if (isSet)
			{
				flags |= (byte)flag;
			}
			else
			{
				flags &= (byte)~flag;
			}

			return flags;
		}

		#endregion

		#endregion
	}
}
