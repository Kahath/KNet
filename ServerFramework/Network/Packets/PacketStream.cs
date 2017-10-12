/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using System;
using UMemory.Unmanaged.Enums;
using UMemory.Unmanaged.Stream.Core;

namespace ServerFramework.Network.Packets
{
	internal class PacketStream : UMemoryStream
	{
		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="PacketStream"/> type.
		/// </summary>
		/// <param name="maxLength">Length of array to allocate on underlying stream.</param>
		public PacketStream(int maxLength)
			: base(maxLength, ServerConfig.Endianness)
		{
		}

		#endregion

		#region Methods

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

			int messageLength = Position - ServerConfig.BigHeaderLength;
			bool isBigHeader = messageLength > UInt16.MaxValue;

			flags = SetupFlag(flags, PacketFlags.BigPacket, isBigHeader);

			int headerLength = isBigHeader ? ServerConfig.BigHeaderLength : ServerConfig.HeaderLength;
			int packetPosition = ServerConfig.BigHeaderLength - headerLength;

			Seek(packetPosition);

			header.Flags = flags;
			header.Length = messageLength;
			header.Opcode = opcode;

			Write(header);
			Adjust(packetPosition);

			header.Flags = flags;
			header.Length = messageLength;
			header.Opcode = opcode;

			return messageLength + headerLength;
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
