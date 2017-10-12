/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Enums;
using ServerFramework.Managers;
using System;
using UMemory.Unmanaged.Enums;

namespace ServerFramework.Network.Packets
{
	public class Packet
	{
		#region Fields

		private PacketHeader _header;
		private PacketStream _stream;
		private int _sessionId;
		private PacketLogTypes _logType;

		#endregion

		#region Properties

		public PacketHeader Header
		{
			get { return _header; }
			internal set { _header = value; }
		}

		public int SessionID
		{
			get { return _sessionId; }
			internal set { _sessionId = value; }
		}

		internal PacketStream Stream
		{
			get { return _stream; }
		}

		internal PacketLogTypes LogType
		{
			get { return _logType; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="Packet"/> type used for reading data.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		internal Packet(PacketLogTypes logType)
		{
			Header = new PacketHeader(0, 0, 0);
			_stream = new PacketStream(0);
			_logType = logType;
		}

		#endregion

		#region Methods

		#region Read

		/// <summary>
		/// Reads generic value from packet stream.
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="item">Object whose data will fill.</param>
		/// <returns>Value of generic type.</returns>
		public T Read<T>(T item = default(T))
		{
			T retVal = default(T);

			try
			{
				retVal = Stream.Read(item);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogTypes.Critical, e);
			}

			return retVal;
		}

		#endregion

		#region Write

		/// <summary>
		/// Writes generic value to packet stream.
		/// </summary>
		/// <typeparam name="T">Type of value</typeparam>
		/// <param name="value">value to write</param>
		public void Write<T>(T value)
		{
			try
			{
				Stream.Write(value);
			}
			catch (IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogTypes.Critical, e);
			}
		}

		#endregion

		#region BitPack

		#region ReadBit

		/// <summary>
		/// Reads one bit from packet stream.
		/// </summary>
		/// <returns>Bit as boolean</returns>
		public bool ReadBit()
		{
			return ReadBits<bool>(1);
		}

		#endregion

		#region WriteBit

		/// <summary>
		/// Writes one bit to packet stream.
		/// </summary>
		/// <param name="value">Value.</param>
		public void WriteBit(bool value)
		{
			WriteBits(value, 1);
		}

		#endregion

		#region ReadBits

		/// <summary>
		/// Reads number of bits from packet stream.
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="count">Number of bits.</param>
		/// <returns>Value of generic type.</returns>
		public T ReadBits<T>(int count) where T
			: struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			return Stream.ReadBits<T>(count);
		}

		#endregion

		#region WriteBits

		/// <summary>
		/// Writes bits to packet stream.
		/// </summary>
		/// <typeparam name="T">Type of value to write.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="count">Number of bits.</param>
		public void WriteBits<T>(T value, int count) where T
			: struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			Stream.WriteBits(value, count);
		}

		/// <summary>
		/// Writes bits with start index to packet stream.
		/// </summary>
		/// <typeparam name="T">Type of value to write.</typeparam>
		/// <param name="value">Value to write.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="count">Number of bits.</param>
		public void WriteBits<T>(T value, int startIndex, int count) where T
			: struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
		{
			Stream.WriteBits(value, startIndex, count);
		}

		#endregion

		#region Flush

		/// <summary>
		/// Writes remaining bits to packet stream.
		/// </summary>
		public void Flush(BitPackFlushType flushType)
		{
			Stream.Flush(flushType);
		}

		#endregion

		#endregion

		#region End

		/// <summary>
		/// Finishes writing data to packet stream.
		/// </summary>
		/// <returns>Length of message array for sending.</returns>
		public int End(byte flags, ushort opcode)
		{
			return Stream.End(Header, flags, opcode);
		}

		#endregion

		#region Alloc

		/// <summary>
		/// Allocates memory for underlying stream.
		/// </summary>
		/// <param name="maxLength">Length to allocate.</param>
		internal void Alloc(int maxLength)
		{
			Stream.Alloc(maxLength);
		}

		/// <summary>
		/// Reallocates memory for underlying stream.
		/// </summary>
		/// <param name="length">Length of memory.</param>
		internal void Realloc(int length)
		{
			Stream.Realloc(length);
		}

		/// <summary>
		/// Resets values for underlying stream and header.
		/// </summary>
		internal void Free()
		{
			Stream.Free();
			Stream.ResetPosition();
			Header.Reset();
		}

		#endregion

		#region Copy

		/// <summary>
		/// Copies data to underlying stream from array.
		/// /// </summary>
		/// <param name="from">Array whose data will be copied.</param>
		/// <param name="fromOffset">Array offset from which data will be copied.</param>
		/// <param name="toOffset">Underlying stream offset from which data will be copied.</param>
		/// <param name="length">Length of array to copy.</param>
		internal void CopyFrom(byte[] from, int fromOffset, int toOffset, uint length)
		{
			try
			{
				Stream.CopyFrom(from, fromOffset, toOffset, length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogTypes.Critical, e);
			}
		}

		/// <summary>
		/// Copies data from underlying stream to array.
		/// </summary>
		/// <param name="srcOffset">Underlying stream offset from which data will be copied.</param>
		/// <param name="to">Array to which data will be copied.</param>
		/// <param name="toOffset">Array offset from which data will be copied.</param>
		/// <param name="length">Length of array to copy.</param>
		internal void CopyTo(int srcOffset, byte[] to, int toOffset, uint length)
		{
			try
			{
				Stream.CopyTo(srcOffset, to, toOffset, length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogTypes.Critical, e);
			}
		}

		#endregion

		#region ToArray

		/// <summary>
		/// Converts packet to byte array.
		/// </summary>
		/// <returns>Packet message as byte array</returns>
		public byte[] ToArray()
		{
			byte[] retVal = new byte[Header.Length];

			try
			{
				CopyTo(0, retVal, 0, (uint)Header.Length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(LogTypes.Critical, e);
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
