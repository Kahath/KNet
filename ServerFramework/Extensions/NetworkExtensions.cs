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

using ServerFramework.Configuration;
using ServerFramework.Network.Packets;
using System;
using System.Net.Sockets;

namespace ServerFramework.Extensions
{
	public static class NetworkExtensions
	{
		#region Methods

		/// <summary>
		/// Handles message header. If received bytes length is lesser than 
		/// header length, multiple method calls are required.
		/// </summary>
		/// <param name="e">SocketAsyncEventArgs object</param>
		/// <param name="token">SocketAsyncEventArgs user token</param>
		/// <param name="remainingBytesToProcess">bytes transfered in receiveCallback</param>
		/// <returns></returns>
		internal static int HandleHeader(this UserToken token
			, SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (token.Header == null)
			{
				token.IsBigPacket = Convert.ToBoolean(e.Buffer[token.HeaderOffset] >> 7);
				token.HeaderLength = token.IsBigPacket ? ServerConfig.BigHeaderLength : ServerConfig.HeaderLength;
				token.Header = new byte[token.HeaderLength];
				token.MessageOffset = token.HeaderOffset + token.HeaderLength;
			}

			if (remainingBytesToProcess >= token.HeaderLength -
				token.HeaderBytesDoneCount)
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	token.HeaderOffset + token.HeaderBytesDoneCount
					,	token.Header
					,	token.HeaderBytesDoneCount
					,	token.HeaderLength - token.HeaderBytesDoneCount
					);

				remainingBytesToProcess = (remainingBytesToProcess - token.HeaderLength) +
					token.HeaderBytesDoneCount;

				token.HeaderBytesDoneThisOp = token.HeaderLength -
					token.HeaderBytesDoneCount;

				token.HeaderBytesDoneCount = token.HeaderLength;

				Array.Reverse(token.Header, 0, token.HeaderLength - ServerConfig.OpcodeLength);

				token.MessageLength = token.IsBigPacket
					? BitConverter.ToInt32(token.Header, 0) & Int32.MaxValue
					: BitConverter.ToInt16(token.Header, 0);

				token.StartReceive();

				token.Packet.Header = new PacketHeader(token.Header, token.IsBigPacket);

				token.IsHeaderReady = true;
			}
			else
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	token.HeaderOffset + token.HeaderBytesDoneCount
					,	token.Header
					,	token.HeaderBytesDoneCount
					,	remainingBytesToProcess
					);

				token.HeaderBytesDoneThisOp = remainingBytesToProcess;
				token.HeaderBytesDoneCount += remainingBytesToProcess;
				remainingBytesToProcess = 0;
			}

			if (remainingBytesToProcess == 0)
			{
				token.MessageOffset -= token.HeaderBytesDoneThisOp;
				token.HeaderBytesDoneThisOp = 0;
			}

			return remainingBytesToProcess;
		}

		/// <summary>
		/// Handles message.  If received bytes length is lesser than 
		/// message length, multiple method calls are required. 
		/// </summary>
		/// <param name="e">SocketAsyncEventArgs object</param>
		/// <param name="token">SocketAsyncEventArgs UserToken</param>
		/// <param name="remainingBytesToProcess">bytes transfered in receive callback</param>
		/// <returns></returns>
		internal static int HandleMessage(this UserToken token,
			SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (token.MessageBytesDoneCount == 0)
				token.Packet.Message = new byte[token.MessageLength];

			if (token.MessageLength == 0)
			{
				token.Packet.SessionId = token.SessionId;
				token.Packet.PrepareRead();

				token.IsPacketReady = true;
			}
			else if ((remainingBytesToProcess +
				token.MessageBytesDoneCount) >=
				token.MessageLength)
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	token.MessageOffset
					,	token.Packet.Message
					,	token.MessageBytesDoneCount
					,	token.MessageLength - token.MessageBytesDoneCount
					);

				remainingBytesToProcess = (remainingBytesToProcess - token.MessageLength)
					+ token.MessageBytesDoneCount;

				token.MessageBytesDoneThisOp = token.MessageLength - token.MessageBytesDoneCount;

				token.Packet.SessionId = token.SessionId;
				token.Packet.PrepareRead();

				token.IsPacketReady = true;
			}
			else
			{
				Buffer.BlockCopy
					(
						e.Buffer
					,	token.MessageOffset
					,	token.Packet.Message
					,	token.MessageBytesDoneCount
					,	remainingBytesToProcess
					);

				token.MessageOffset -= token.HeaderBytesDoneThisOp;
				token.MessageBytesDoneCount += remainingBytesToProcess;

				remainingBytesToProcess = 0;
			}

			return remainingBytesToProcess;
		}

		#endregion
	}
}
