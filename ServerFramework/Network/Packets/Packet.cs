/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Managers;
using System;

namespace ServerFramework.Network.Packets
{
	public class Packet
	{
		#region Fields

		private PacketHeader _header;
		private PacketStream _stream;
		private int _sessionId;
		private PacketLogType _logType;

		#endregion

		#region Properties

		public PacketHeader Header
		{
			get { return _header; }
			internal set { _header = value; }
		}

		public int SessionId
		{
			get { return _sessionId; }
			internal set { _sessionId = value; }
		}

		internal PacketStream Stream
		{
			get { return _stream; }
		}

		internal PacketLogType LogType
		{
			get { return _logType; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Network.Packets.Packet"/> type used for reading data.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		internal Packet(PacketLogType logType)
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
		/// <param name="count">Length to read if T is array.</param>
		/// <returns>Value of generic type.</returns>
		public T Read<T>(int count = 0)
		{
			T retVal = default(T);
			try
			{
				retVal = Stream.Read<T>();
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(Enums.LogType.Critical, e.ToString());
				Environment.Exit(-1);
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
				Stream.Write<T>(value);
			}
			catch (IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(Enums.LogType.Critical, e.ToString());
				Environment.Exit(-1);
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
			WriteBits<bool>(value, 1);
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
			Stream.WriteBits<T>(value, count);
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
			Stream.WriteBits<T>(value, startIndex, count);
		}

		#endregion

		#region Flush

		/// <summary>
		/// Writes remaining bits to packet stream.
		/// </summary>
		public void Flush()
		{
			Stream.Flush();
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
			int retVal;

			Stream.End(Header, flags, opcode);
			retVal = Header.Length + (Header.IsBigHeader ? ServerConfig.BigHeaderLength : ServerConfig.HeaderLength);

			return retVal;
		}

		#endregion

		#region Alloc

		internal void Alloc(int maxLength)
		{
			Stream.Alloc(maxLength);
		}

		internal void Realloc(int length)
		{
			Stream.Realloc(length);
		}

		internal void Free()
		{
			Stream.Free();
			Stream.ResetPosition();
			Header.Reset();
		}

		#endregion

		#region Copy

		internal void CopyFrom(byte[] from, int fromOffset, int toOffset, uint length)
		{
			try
			{
				Stream.CopyFrom(from, fromOffset, toOffset, length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(Enums.LogType.Critical, e.ToString());
				Environment.Exit(-1);
			}
		}

		internal void CopyTo(int srcOffset, byte[] to, int toOffset, uint length)
		{
			try
			{
				Stream.CopyTo(srcOffset, to, toOffset, length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(Enums.LogType.Critical, e.ToString());
				Environment.Exit(-1);
			}
		}

		#endregion

		#region ToArray

		public byte[] ToArray()
		{
			byte[] retVal = new byte[Header.Length];

			try
			{
				CopyTo(0, retVal, 0, (uint)Header.Length);
			}
			catch(IndexOutOfRangeException e)
			{
				Manager.LogMgr.Log(Enums.LogType.Critical, e.ToString());
				Environment.Exit(-1);
			}

			return retVal;
		}

		#endregion

		#endregion
	}
}
