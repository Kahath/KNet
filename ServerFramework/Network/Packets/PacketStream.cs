/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using System;
using UMemory.Unmanaged.Stream.Core;

namespace ServerFramework.Network.Packets
{
	internal class PacketStream : UMemoryStream 
	{
		#region Fields

		private byte _bitPosition = 0;
		private byte _value;

		#endregion

		#region Properties

		private byte BitPosition
		{
			get { return _bitPosition; }
			set { _bitPosition = value; }
		}

		private byte Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Constructors

		public PacketStream(int maxLength)
			: base(maxLength)
		{
			
		}

		#endregion

		#region Methods

		#region BitPack

		#region ReadBit

		/// <summary>
		/// Reads one bit from underlying stream.
		/// </summary>
		/// <returns>Bit as boolean.</returns>
		private bool ReadBit()
		{
			if (BitPosition == 0)
			{
				Value = Read<byte>();
				BitPosition = 8;
			}

			bool retVal = Convert.ToBoolean(Value >> 7);

			--BitPosition;
			Value <<= 1;

			return retVal;
		}

		#endregion

		#region ReadBits

		/// <summary>
		/// Reads number of bits from underlying stream
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="count">Number of bits.</param>
		/// <returns>Value of generic type.</returns>
		internal T ReadBits<T>(int count)
		{
			int retVal = 0;

			for (int i = count - 1; i >= 0; --i)
				retVal = ReadBit() ? (1 << i) | retVal : retVal;

			return (T)Convert.ChangeType(retVal, typeof(T));
		}

		#endregion

		#region WriteBit

		/// <summary>
		/// Writes one bit to underlying stream.
		/// </summary>
		/// <param name="value">Value.</param>
		private void WriteBit(bool value)
		{
			++BitPosition;

			if (value)
				Value |= (byte)(1 << (8 - BitPosition));

			if (BitPosition == 8)
			{
				Write<byte>(Value);
				BitPosition = 0;
				Value = 0;
			}
		}

		#endregion

		#region WriteBits

		/// <summary>
		/// Writes number of bits to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="count">Number of bits.</param>
		internal void WriteBits<T>(T value, int count)
		{
			for (int i = count - 1; i >= 0; --i)
				WriteBit((bool)Convert.ChangeType(
					(Convert.ToInt32(value) >> i) & 1, typeof(bool)));
		}

		/// <summary>
		/// Writes number of bits with start index to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="count">Number of bits.</param>
		internal void WriteBits<T>(T value, int startIndex, int count)
		{
			for (int i = startIndex + count - 1; i >= startIndex; --i)
				WriteBit((bool)Convert.ChangeType(
					(Convert.ToInt32(value) >> i) & 1, typeof(bool)));
		}

		#endregion

		#region Flush

		/// <summary>
		/// Writes remaining bits to underlying stream.
		/// </summary>
		internal void Flush()
		{
			if (BitPosition != 0)
			{
				Write<byte>(Value);

				BitPosition = 0;
				Value = 0;
			}
		}

		#endregion

		#endregion

		#region End

		internal PacketHeader End(byte flags, ushort opcode)
		{
			PacketHeader retVal  = null;
			Flush();

			int messageLength = Position - ServerConfig.BigHeaderLength;
			bool isBigHeader = messageLength > UInt16.MaxValue;

			flags = SetupFlag(flags, PacketFlag.BigPacket, isBigHeader);

			int headerLength = isBigHeader
				? ServerConfig.BigHeaderLength
				: ServerConfig.HeaderLength;

			int packetPosition = ServerConfig.BigHeaderLength - headerLength;

			Seek(packetPosition);

			Write<byte>(flags);

			if (!isBigHeader)
				Write<ushort>((ushort)messageLength);
			else
				Write<int>(messageLength);

			Write<ushort>(opcode);

			Adjust(packetPosition);

			retVal = new PacketHeader(flags, messageLength, opcode);		

			return retVal;
		}

		#endregion

		#region ResetPosition

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
		public byte SetupFlag(byte flags, PacketFlag flag, bool isSet)
		{
			if (isSet)
				flags |= (byte)flag;
			else
				flags &= (byte)~flag;

			return flags;
		}

		#endregion

		#endregion
	}
}
