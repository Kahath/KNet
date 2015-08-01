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
using System;
using System.IO;
using System.Text;

namespace ServerFramework.Network.Packets
{
	public class Packet : IDisposable
	{
		#region Fields

		private PacketHeader _header;
		private PacketStream _stream;
		private Encoding _encoder;
		private byte[] _message;
		private int _sessionId;

		#endregion

		#region Properties

		public PacketHeader Header
		{
			get { return _header; }
			internal set { _header = value; }
		}

		public byte[] Message
		{
			get { return _message; }
			internal set { _message = value; }
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

		private Encoding Encoding
		{
			get { return _encoder; }
			set { _encoder = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Network.Packets.Packet"/> type used for reading data.
		/// </summary>
		/// <param name="header">Header byte array.</param>
		internal Packet(byte[] header)
		{
			Header = new PacketHeader(header);
			Encoding = Header.IsUnicode ? Encoding.Unicode : Encoding.UTF8;
		}

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Network.Packets.Packet"/> type used for writing data.
		/// </summary>
		/// <param name="opcode">Operational code.</param>
		/// <param name="encoding">Encoding used in packet.</param>
		/// <param name="flags">Flags <see cref="ServerFramework.Constants.Misc.PacketFlag"/>.</param>
		public Packet(ushort opcode, Encoding encoding, byte flags = 0)
		{
			Encoding = encoding ?? Encoding.UTF8;
			_stream = new PacketStream(Encoding);

			Write<byte>(flags);
			Write<int>(ServerConfig.BigHeaderLength);
			Write<ushort>(opcode);
		}

		#endregion

		#region Methods

		#region PrepareRead

		/// <summary>
		/// Prepares packet stream for reading data.
		/// </summary>
		internal void PrepareRead()
		{
			_stream = new PacketStream(Encoding, this.Message);
		}

		#endregion

		#region Read

		/// <summary>
		/// Reads generic value from packet stream.
		/// </summary>
		/// <typeparam name="T">Type of return value.</typeparam>
		/// <param name="count">Length to read if T is array.</param>
		/// <returns>Value of generic type.</returns>
		public T Read<T>(int count = 0)
		{
			return Stream.Read<T>(count);
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
			Stream.Write<T>(value);
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
		public int End()
		{
			byte[] header;

			int retVal = Stream.End(out _message, out header);

			Header = new PacketHeader(header);

			return retVal;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes object clearing managed resources.
		/// </summary>
		/// <param name="isDisposing">Clear managed resources.</param>
		private void Dispose(bool isDisposing)
		{
			if(isDisposing)
			{
				_stream.Dispose();
			}
		}

		#endregion

		#endregion
	}
}
