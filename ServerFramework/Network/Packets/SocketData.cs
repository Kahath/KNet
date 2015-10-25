/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using System;
using System.Net.Sockets;

namespace ServerFramework.Network.Packets
{
	internal sealed class SocketData
	{
		#region Fields

		private Packet _packet;

		private readonly int _bufferOffset;
		private readonly int _bufferSize;
		private int _sessionId;

		private int _messageLength;
		private int _messageOffset;
		private int _headerLength;
		private int _headerOffset;

		private int _headerBytesDoneCount = 0;
		private int _messageBytesDoneCount = 0;
		private int _headerBytesRemainingCount = 0;
		private int _messageBytesRemainingCount = 0;
		private int _headerBytesDoneThisOp = 0;
		private int _messageBytesDoneThisOp = 0;

		private bool _isHeaderReady = false;
		private bool _isPacketReady = false;
		private bool _isBigPacket = false;
		private bool _isUnicode = false;

		#endregion

		#region Properties

		internal int MessageLength
		{
			get { return _messageLength; }
			set { _messageLength = value; }
		}

		internal int MessageOffset
		{
			get { return _messageOffset; }
			set { _messageOffset = value; }
		}

		internal int HeaderOffset
		{
			get { return _headerOffset; }
			set { _headerOffset = value; }
		}

		internal int HeaderLength
		{
			get { return _headerLength; }
			set { _headerLength = value; }
		}

		internal int BytesDoneCount
		{
			get { return HeaderBytesDoneCount + MessageBytesDoneCount; }
		}

		internal int HeaderBytesDoneCount
		{
			get { return _headerBytesDoneCount; }
			set { _headerBytesDoneCount = value; }
		}

		internal int MessageBytesDoneCount
		{
			get { return _messageBytesDoneCount; }
			set { _messageBytesDoneCount = value; }
		}

		internal int HeaderBytesRemainingCount
		{
			get { return _headerBytesRemainingCount; }
			set { _headerBytesRemainingCount = value; }
		}

		internal int MessageBytesRemainingCount
		{
			get { return _messageBytesRemainingCount; }
			set { _messageBytesRemainingCount = value; }
		}

		internal int HeaderBytesDoneThisOp
		{
			get { return _headerBytesDoneThisOp; }
			set { _headerBytesDoneThisOp = value; }
		}

		internal int MessageBytesDoneThisOp
		{
			get { return _messageBytesDoneThisOp; }
			set { _messageBytesDoneThisOp = value; }
		}

		internal bool IsHeaderReady
		{
			get { return _isHeaderReady; }
			set { _isHeaderReady = value; }
		}

		internal bool IsPacketReady
		{
			get { return _isPacketReady; }
			set { _isPacketReady = value; }
		}

		internal bool IsBigPacket
		{
			get { return _isBigPacket; }
			set { _isBigPacket = value; }
		}

		internal bool IsUnicode
		{
			get { return _isUnicode; }
			set { _isUnicode = value; }
		}

		internal int BufferSize
		{
			get { return _bufferSize; }
		}

		internal int BufferOffset
		{
			get { return _bufferOffset; }
		}

		internal Packet Packet
		{
			get { return _packet; }
			set { _packet = value; }
		}

		public int SessionId
		{
			get { return _sessionId; }
			internal set { _sessionId = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Network.Packets.SocketData"/> type.
		/// </summary>
		/// <param name="bufferSize">Buffer size for client.</param>
		/// <param name="bufferOffset">Buffer offset in large alocated buffer.</param>
		/// <param name="headerLength">Length of message header.</param>
		internal SocketData(int bufferSize, int bufferOffset, int headerLength, PacketLogType logType)
		{
			_bufferSize = bufferSize;
			_bufferOffset = bufferOffset;
			HeaderLength = headerLength;
			HeaderOffset = bufferOffset;
			Packet = new Packet(logType);
		}

		#endregion

		#region Methods

		#region Finish

		/// <summary>
		/// Finishes writing data to packet.
		/// </summary>
		internal void Finish(byte flags, ushort opcode)
		{
			MessageBytesRemainingCount = Packet.End(flags, opcode);
		}

		#endregion

		#region HandleHeader

		/// <summary>
		/// Handles message header. If received bytes length is lesser than 
		/// header length, multiple method calls are required.
		/// </summary>
		/// <param name="e">SocketAsyncEventArgs object</param>
		/// <param name="data">SocketAsyncEventArgs user token</param>
		/// <param name="remainingBytesToProcess">bytes transfered in receiveCallback</param>
		/// <returns></returns>
		internal int HandleHeader(SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (HeaderBytesDoneCount == 0)
			{
				byte flags = e.Buffer[HeaderOffset];

				IsBigPacket = Convert.ToBoolean(flags & (byte)PacketFlag.BigPacket);

				HeaderLength = IsBigPacket
					? ServerConfig.BigHeaderLength
					: ServerConfig.HeaderLength;

				MessageOffset = HeaderOffset + HeaderLength;

				Packet.Alloc(HeaderLength);
			}

			if (remainingBytesToProcess >= HeaderLength - HeaderBytesDoneCount)
			{
				Packet.CopyFrom(e.Buffer, HeaderOffset + HeaderBytesDoneCount, HeaderBytesDoneCount, (uint)(HeaderLength - HeaderBytesDoneCount));
				remainingBytesToProcess = (remainingBytesToProcess - HeaderLength) + HeaderBytesDoneCount;

				HeaderBytesDoneThisOp = HeaderLength - HeaderBytesDoneCount;
				HeaderBytesDoneCount = HeaderLength;

				Packet.Header.Flags = Packet.Read<byte>();
				Packet.Header.Length = IsBigPacket ? Packet.Read<int>() : Packet.Read<ushort>();
				Packet.Header.Opcode = Packet.Read<ushort>();
		
				MessageLength = Packet.Header.Length;

				if(MessageLength > 0)
					Packet.Realloc(HeaderLength + MessageLength);

				IsHeaderReady = true;
			}
			else
			{
				Packet.CopyFrom(e.Buffer, HeaderOffset + HeaderBytesDoneCount, HeaderBytesDoneCount, (uint)remainingBytesToProcess);

				HeaderBytesDoneThisOp = remainingBytesToProcess;
				HeaderBytesDoneCount += remainingBytesToProcess;
				remainingBytesToProcess = 0;
			}

			if (remainingBytesToProcess == 0)
			{
				MessageOffset -= HeaderBytesDoneThisOp;
				HeaderBytesDoneThisOp = 0;
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
		internal int HandleMessage(SocketAsyncEventArgs e, int remainingBytesToProcess)
		{
			if (MessageLength == 0)
			{
				Packet.SessionId = SessionId;
				IsPacketReady = true;
			}
			else if ((remainingBytesToProcess + MessageBytesDoneCount) >= MessageLength)
			{
				Packet.CopyFrom(e.Buffer, MessageOffset, BytesDoneCount, (uint)(MessageLength - MessageBytesDoneCount));

				remainingBytesToProcess = (remainingBytesToProcess - MessageLength) + MessageBytesDoneCount;
				MessageBytesDoneThisOp = MessageLength - MessageBytesDoneCount;

				Packet.SessionId = SessionId;
				IsPacketReady = true;
			}
			else
			{
				Packet.CopyFrom(e.Buffer, MessageOffset, BytesDoneCount, (uint)remainingBytesToProcess);

				MessageOffset -= HeaderBytesDoneThisOp;
				MessageBytesDoneCount += remainingBytesToProcess;

				remainingBytesToProcess = 0;
			}

			return remainingBytesToProcess;
		}

		#endregion

		#region Reset

		/// <summary>
		/// Resets user token to its initial state.
		/// </summary>
		/// <param name="headerOffset">Header offset</param>
		internal void Reset(int headerOffset)
		{
			Packet.Free();
			IsHeaderReady = false;
			IsPacketReady = false;
			IsBigPacket = false;
			IsUnicode = false;
			HeaderBytesDoneCount = 0;
			HeaderBytesRemainingCount = 0;
			HeaderBytesDoneThisOp = 0;
			MessageBytesDoneCount = 0;
			MessageBytesRemainingCount = 0;
			MessageBytesDoneThisOp = 0;
			MessageLength = 0;
			HeaderLength = 0;
			HeaderOffset = headerOffset;
		}

		#endregion

		#endregion
	}
}
