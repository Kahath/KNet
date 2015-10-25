/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Network.Packets;
using System;
using System.Net.Sockets;

namespace ServerFramework.Extensions
{
	public static class NetworkExtensions
	{
		#region Methods

		#region HandleHeader

		/// <summary>
		/// Handles message header. If received bytes length is lesser than 
		/// header length, multiple method calls are required.
		/// </summary>
		/// <param name="e">SocketAsyncEventArgs object</param>
		/// <param name="data">SocketAsyncEventArgs user token</param>
		/// <param name="remainingBytesToProcess">bytes transfered in receiveCallback</param>
		/// <returns></returns>
		internal static int HandleHeader(this SocketData data
			, SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (data.HeaderBytesDoneCount == 0)
			{
				byte flags = e.Buffer[data.HeaderOffset];

				data.IsBigPacket = Convert.ToBoolean(flags & (byte)PacketFlag.BigPacket);
				data.IsUnicode = Convert.ToBoolean(flags & (byte)PacketFlag.Unicode);

				data.HeaderLength = data.IsBigPacket
					? ServerConfig.BigHeaderLength
					: ServerConfig.HeaderLength;

				data.MessageOffset = data.HeaderOffset + data.HeaderLength;
			}

			if (remainingBytesToProcess >= data.HeaderLength -
				data.HeaderBytesDoneCount)
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	data.HeaderOffset + data.HeaderBytesDoneCount
					,	data.Header
					,	data.HeaderBytesDoneCount
					,	data.HeaderLength - data.HeaderBytesDoneCount
					);

				remainingBytesToProcess = (remainingBytesToProcess - data.HeaderLength) +
					data.HeaderBytesDoneCount;

				data.HeaderBytesDoneThisOp = data.HeaderLength -
					data.HeaderBytesDoneCount;

				data.HeaderBytesDoneCount = data.HeaderLength;

				data.Packet = new Packet(data.Header);
				data.MessageLength = data.Packet.Header.Length;

				data.IsHeaderReady = true;
			}
			else
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	data.HeaderOffset + data.HeaderBytesDoneCount
					,	data.Header
					,	data.HeaderBytesDoneCount
					,	remainingBytesToProcess
					);

				data.HeaderBytesDoneThisOp = remainingBytesToProcess;
				data.HeaderBytesDoneCount += remainingBytesToProcess;
				remainingBytesToProcess = 0;
			}

			if (remainingBytesToProcess == 0)
			{
				data.MessageOffset -= data.HeaderBytesDoneThisOp;
				data.HeaderBytesDoneThisOp = 0;
			}

			return remainingBytesToProcess;
		}

		#endregion

		#region HandleMessage

		/// <summary>
		/// Handles message.  If received bytes length is lesser than 
		/// message length, multiple method calls are required. 
		/// </summary>
		/// <param name="e">SocketAsyncEventArgs object</param>
		/// <param name="data">SocketAsyncEventArgs UserToken</param>
		/// <param name="remainingBytesToProcess">bytes transfered in receive callback</param>
		/// <returns></returns>
		internal static int HandleMessage(this SocketData data,
			SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (data.MessageBytesDoneCount == 0)
				data.Packet.Message = new byte[data.MessageLength];

			if (data.MessageLength == 0)
			{
				data.Packet.SessionId = data.SessionId;
				data.Packet.PrepareRead();

				data.IsPacketReady = true;
			}
			else if ((remainingBytesToProcess +
				data.MessageBytesDoneCount) >=
				data.MessageLength)
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	data.MessageOffset
					,	data.Packet.Message
					,	data.MessageBytesDoneCount
					,	data.MessageLength - data.MessageBytesDoneCount
					);

				remainingBytesToProcess = (remainingBytesToProcess - data.MessageLength)
					+ data.MessageBytesDoneCount;

				data.MessageBytesDoneThisOp = data.MessageLength - data.MessageBytesDoneCount;

				data.Packet.SessionId = data.SessionId;
				data.Packet.PrepareRead();

				data.IsPacketReady = true;
			}
			else
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	data.MessageOffset
					,	data.Packet.Message
					,	data.MessageBytesDoneCount
					,	remainingBytesToProcess
					);

				data.MessageOffset -= data.HeaderBytesDoneThisOp;
				data.MessageBytesDoneCount += remainingBytesToProcess;

				remainingBytesToProcess = 0;
			}

			return remainingBytesToProcess;
		}

		#endregion
		
		#endregion
	}
}
