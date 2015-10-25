/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Enums;
using ServerFramework.Extensions;
using System;
using System.IO;
using System.Text;

namespace ServerFramework.Network.Packets
{
	internal class PacketStream : IDisposable
	{
		#region Fields

		private BinaryReader _reader;
		private BinaryWriter _writer;
		private Encoding _encoder;

		private byte _position = 0;
		private byte _value;

		#endregion

		#region Properties

		internal BinaryReader Reader
		{
			get { return _reader; }
			set { _reader = value; }
		}

		internal BinaryWriter Writer
		{
			get { return _writer; }
			set { _writer = value; }
		}

		internal Encoding Encoder
		{
			get { return _encoder; }
			set { _encoder = value; }
		}

		private byte Position
		{
			get { return _position; }
			set { _position = value; }
		}

		private byte Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Network.Packets.PacketStream"/> type.
		/// </summary>
		/// <param name="encoding">Encoding used in underlying stream.</param>
		/// <param name="data">Data to read if stream should instance as reader.</param>
		public PacketStream(Encoding encoding, byte[] data = null)
		{
			Encoder = encoding;

			if (data != null)
				Reader = new BinaryReader(new MemoryStream(data, false), encoding);
			else
				Writer = new BinaryWriter(new MemoryStream(), encoding);
		}

		#endregion

		#region Methods

		#region Read

		/// <summary>
		/// Reads generic value from underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="count">Length to read if T is array.</param>
		/// <returns>Value of generic type</returns>
		internal T Read<T>(int count = 0)
		{
			T retVal = default(T);

			if (Reader != null)
				retVal = Reader.Read<T>(count);
			else
				throw new NullReferenceException("Reader cannot be null");

			return retVal;
		}

		#endregion

		#region Write

		/// <summary>
		/// Writes generic value to underlying stream.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value.</param>
		internal void Write<T>(T value)
		{
			if (Writer != null)
			{
				T trueValue = (T)Convert.ChangeType(value, typeof(T));
				Writer.Write<T>(trueValue);
			}
			else
			{
				throw new NullReferenceException("Writer cannot be null");
			}
		}

		#endregion

		#region BitPack

		#region ReadBit

		/// <summary>
		/// Reads one bit from underlying stream.
		/// </summary>
		/// <returns>Bit as boolean.</returns>
		private bool ReadBit()
		{
			if (Position == 0)
			{
				Value = Read<byte>();
				Position = 8;
			}

			bool retVal = Convert.ToBoolean(Value >> 7);

			--Position;
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
			++Position;

			if (value)
				Value |= (byte)(1 << (8 - Position));

			if (Position == 8)
			{
				Write<byte>(Value);
				Position = 0;
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
			if (Position != 0)
			{
				Write<byte>(Value);

				Position = 0;
				Value = 0;
			}
		}

		#endregion

		#endregion

		#region End

		/// <summary>
		/// Finishes writing data to underlying stream.
		/// </summary>
		/// <param name="message">Packet message.</param>
		/// <param name="header">Packet header.</param>
		/// <returns>message + header array length.</returns>
		internal int End(out byte[] message, out byte[] header)
		{
			Flush();

			Writer.BaseStream.Seek(0, SeekOrigin.Begin);
			byte flags = Convert.ToByte(Writer.BaseStream.ReadByte());
			
			Writer.BaseStream.Seek(0, SeekOrigin.Begin);

			int length = (int)Writer.BaseStream.Length;
			int messageLength = length - ServerConfig.BigHeaderLength;

			bool isBigHeader = messageLength > UInt16.MaxValue;
			bool isUnicode = Encoder == Encoding.Unicode;
			
			int headerLength = isBigHeader
				? ServerConfig.BigHeaderLength
				: ServerConfig.HeaderLength;

			message = new byte[messageLength + headerLength];

			Writer.BaseStream.Skip(ServerConfig.BigHeaderLength - headerLength);
			Writer.BaseStream.Read(message, 0, messageLength + headerLength);

			flags = SetupFlag(flags, PacketFlag.BigPacket, isBigHeader);
			flags = SetupFlag(flags, PacketFlag.Unicode, isUnicode);

			message.SetValue(flags, 0);
			message.SetValue((byte)(messageLength & Byte.MaxValue), 1);
			message.SetValue((byte)((messageLength >> 8) & Byte.MaxValue), 2);

			if (isBigHeader)
			{				
				message.SetValue((byte)((messageLength >> 16) & Byte.MaxValue), 3);
				message.SetValue((byte)((messageLength >> 24) & Byte.MaxValue), 4);
			}

			header = new byte[headerLength];

			Array.Copy(message, header, headerLength);

			return message.Length;
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

		#region Dispose

		/// <summary>
		/// Disposes object rsources.
		/// </summary>
		public void Dispose()
		{
			if (Reader != null)
				Reader.Close();

			if (Writer != null)
				Writer.Close();
		}

		#endregion

		#endregion
	}
}
