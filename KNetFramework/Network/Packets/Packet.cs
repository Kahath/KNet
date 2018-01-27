/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Enums;
using KNetFramework.Managers;
using System;
using UMemory.Unmanaged.Stream.Base;

namespace KNetFramework.Network.Packets
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


		#region Write

		/// <summary>
		/// Writes <see cref="IUMemoryWrite"/> instance to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of instance to write.</typeparam>
		/// <param name="data">Instance to write.</param>
		public void Write<T>(T data) where T : IUMemoryWrite
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes boolean value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Boolean value to write.</param>
		public void Write(bool data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes byte value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Byte value to write.</param>
		public void Write(byte data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes char value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Char value to write.</param>
		public void Write(char data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes unsigned short value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned short value to write.</param>
		public void Write(ushort data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes unsigned int value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned int value to write.</param>
		public void Write(uint data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes unsigned long value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned long value to write.</param>
		public void Write(ulong data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes short value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Short value to write.</param>
		public void Write(short data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes int value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Int value to write.</param>
		public void Write(int data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes long value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Long value to write.</param>
		public void Write(long data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes float value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Float value to write.</param>
		public void Write(float data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes double value to underlying stream.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Double value to write.</param>
		public void Write(double data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes string value to underlying stream.
		/// </summary>
		/// <param name="data">string value to write.</param>
		public void Write(string data)
		{
			Stream.Write(data);
		}

		/// <summary>
		/// Writes byte array value to underlying stream.
		/// </summary>
		/// <param name="data">Byte array value to write.</param>
		public void Write(byte[] data)
		{
			Stream.Write(data);
		}

		#endregion

		#region Read

		/// <summary>
		/// Reads <see cref="IUMemoryRead" /> instance from underlying stream.
		/// If instance is null, new object is created.
		/// </summary>
		/// <typeparam name="T">Type of instance to read.</typeparam>
		/// <param name="value">instance to read.</param>
		/// <returns>Read instance from stream.</returns>
		public T Read<T>(T value = default(T)) where T : IUMemoryRead, new()
		{
			return Stream.Read(value);
		}

		/// <summary>
		/// Reads boolean value from underlying stream.
		/// </summary>
		/// <returns>Read boolean value.</returns>
		public bool ReadBoolean()
		{
			return Stream.ReadBoolean();
		}

		/// <summary>
		/// Reads byte value from underlying stream.
		/// </summary>
		/// <returns>Read byte value.</returns>
		public byte ReadByte()
		{
			return Stream.ReadByte();
		}

		/// <summary>
		/// Reads signed byte value from underlying stream.
		/// </summary>
		/// <returns>Read signed byte value.</returns>
		public sbyte ReadSByte()
		{
			return Stream.ReadSByte();
		}

		/// <summary>
		/// Reads char value from underlying stream.
		/// </summary>
		/// <returns>Read char value.</returns>
		public char ReadChar()
		{
			return Stream.ReadChar();
		}

		/// <summary>
		/// Reads unsigned short value from underlying stream.
		/// </summary>
		/// <returns>Read unsigned short value.</returns>
		public ushort ReadUInt16()
		{
			return Stream.ReadUInt16();
		}

		/// <summary>
		/// Reads short value from underlying stream.
		/// </summary>
		/// <returns>Read short value.</returns>
		public short ReadInt16()
		{
			return Stream.ReadInt16();
		}

		/// <summary>
		/// Reads unsigned int value from underlying stream.
		/// </summary>
		/// <returns>Read unsigned int value.</returns>
		public uint ReadUInt32()
		{
			return Stream.ReadUInt32();
		}

		/// <summary>
		/// Reads int value from underlying stream.
		/// </summary>
		/// <returns>Read int value.</returns>
		public int ReadInt32()
		{
			return Stream.ReadInt32();
		}

		/// <summary>
		/// Reads unsigned long value from underlying stream.
		/// </summary>
		/// <returns>Read unsigned long value.</returns>
		public ulong ReadUInt64()
		{
			return Stream.ReadUInt64();
		}

		/// <summary>
		/// Reads long value from underlying stream.
		/// </summary>
		/// <returns>Read long value.</returns>
		public long ReadInt64()
		{
			return Stream.ReadInt64();
		}

		/// <summary>
		/// Reads float value from underlying stream.
		/// </summary>
		/// <returns>Read long value.</returns>
		public float ReadFloat()
		{
			return Stream.ReadFloat();
		}

		/// <summary>
		/// Reads double value from underlying stream.
		/// </summary>
		/// <returns>Read double value.</returns>
		public double ReadDouble()
		{
			return Stream.ReadDouble();
		}

		/// <summary>
		/// Reads string from underlying stream.
		/// </summary>
		/// <returns>Read string value.</returns>
		public string ReadString()
		{
			return Stream.ReadString();
		}

		/// <summary>
		/// Reads byte array from underlying stream.
		/// </summary>
		/// <returns>Read byte array value.</returns>
		public byte[] ReadByteArray()
		{
			return Stream.ReadByteArray();
		}

		#endregion

		#region BitPack

		#region Write

		/// <summary>
		/// Reads one bit from packet stream.
		/// </summary>
		/// <returns>Bit as boolean</returns>
		public bool ReadBit()
		{
			return ReadBits<bool>(1);
		}

		/// <summary>
		/// Writes one bit to packet stream.
		/// </summary>
		/// <param name="value">Value.</param>
		public void WriteBit(bool value)
		{
			WriteBits(value, 1);
		}

		#endregion

		#region Read

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
			catch (IndexOutOfRangeException e)
			{
				Manager.LogManager.Log(LogTypes.Critical, e);
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
			catch (IndexOutOfRangeException e)
			{
				Manager.LogManager.Log(LogTypes.Critical, e);
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
			catch (IndexOutOfRangeException e)
			{
				Manager.LogManager.Log(LogTypes.Critical, e);
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
