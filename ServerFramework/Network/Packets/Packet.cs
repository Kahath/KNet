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

using System;
using System.IO;
using System.Text;

namespace ServerFramework.Network.Packets
{
    public sealed class Packet : IDisposable
    {
        #region Fields

        private PacketHeader _header;
        private byte[] _message;
        private int _sessionId;

        private byte _position = 0;
        private byte _value;

        private dynamic _stream;

        #endregion

        #region Properties

        public PacketHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public byte[] Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public int SessionId
        {
            get { return _sessionId; }
            internal set { _sessionId = value; }
        }

        public byte Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new object for reading message.
        /// </summary>
        internal Packet()
        {
            Header = new PacketHeader();
        }

        /// <summary>
        /// Creates new object for writing message
        /// </summary>
        /// <param name="message">opcode of message</param>
        public Packet(ushort message)
        {
            _stream = new BinaryWriter(new MemoryStream());
            Header = new PacketHeader
            {
                Size = 4,
                Opcode = message
            };

            Write<ushort>(Header.Size);
            Write<ushort>(Header.Opcode);
        }

        #endregion

        #region Methods

        #region PrepareRead

        /// <summary>
        /// Readies buffer for reading
        /// </summary>
        internal void PrepareRead()
        {
            if (!(_stream is BinaryWriter))
                _stream = new BinaryReader(new MemoryStream(this.Message));
        }

        #endregion

        #region Read

        /// <summary>
        /// Used for reading from packet buffer
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="count">not used</param>
        /// <returns>Byte, SByte, UInt16, Int16, UInt32, Int32,
        /// UInt64, Int64, Char, Double, Single, Boolea, Pascal String
        /// depending of method type</returns>
        public T Read<T>(int count = 0)
        {
            if (_stream is BinaryWriter)
                return default(T);

            switch (typeof(T).Name)
            {
                case "Byte":
                    return _stream.ReadByte();
                case "SByte":
                    return _stream.ReadSByte();
                case "UInt16":
                    return _stream.ReadUInt16();
                case "Int16":
                    return _stream.ReadInt16();
                case "UInt32":
                    return _stream.ReadUInt32();
                case "Int32":
                    return _stream.ReadInt32();
                case "UInt64":
                    return _stream.ReadUInt64();
                case "Int64":
                    return _stream.ReadInt64();
                case "Char":
                    return _stream.ReadChar();
                case "Double":
                    return _stream.ReadDouble();
                case "Single":
                    return _stream.ReadSingle();
                case "Boolean":
                    return _stream.ReadBoolean();
                case "String":
                    var bytes = _stream.ReadBytes(ReadBits<byte>(count));
                    return Encoding.UTF8.GetString(bytes);
                case "Byte[]":
                    return _stream.ReadBytes(count);
                default:
                    return default(T);
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// Writes value to stream buffer.
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="value">value of method type</param>
        public void Write<T>(T value)
        {
            if (_stream is BinaryReader)
                return;

            switch (typeof(T).Name)
            {
                case "Byte":
                    _stream.Write(Convert.ToByte(value));
                    break;
                case "SByte":
                    _stream.Write(Convert.ToSByte(value));
                    break;
                case "UInt16":
                    _stream.Write(Convert.ToUInt16(value));
                    break;
                case "Int16":
                    _stream.Write(Convert.ToInt16(value));
                    break;
                case "UInt32":
                    _stream.Write(Convert.ToUInt32(value));
                    break;
                case "Int32":
                    _stream.Write(Convert.ToInt32(value));
                    break;
                case "UInt64":
                    _stream.Write(Convert.ToUInt64(value));
                    break;
                case "Int64":
                    _stream.Write(Convert.ToInt64(value));
                    break;
                case "Single":
                    _stream.Write(Convert.ToSingle(value));
                    break;
                case "String":
                    var data = Encoding.UTF8.GetBytes(value as string);
                    _stream.Write(Convert.ToByte(data.Length));
                    _stream.Write(data);
                    break;
                case "Byte[]":
                    data = value as byte[];

                    if (data != null)
                        _stream.Write(data);
                    break;
            }
        }

        #endregion

        #region BitPack

        #region ReadBit

        public bool ReadBit()
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

        public T ReadBits<T>(int count)
        {
            int retVal = 0;

            for (int i = count - 1; i >= 0; --i)
                retVal = ReadBit() ? (1 << i) | retVal : retVal;

            return (T)Convert.ChangeType(retVal, typeof(T));
        }

        #endregion

        #region WriteBit

        public void WriteBit(bool value)
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

        public void WriteBits<T>(T value, int count)
        {
            for (int i = count - 1; i >= 0; --i)
                WriteBit((bool)Convert.ChangeType(
                    (Convert.ToInt32(value) >> i) & 1, typeof(bool)));
        }

        #endregion

        #region Flush

        public void Flush()
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
        /// Readies packet for sending.
        /// </summary>
        /// <returns>Size of packet minus header size</returns>
        internal int End()
        {
            _stream.BaseStream.Seek(0, SeekOrigin.Begin);
            Message = new byte[_stream.BaseStream.Length];
            Header.Size = (ushort)(Message.Length - 4);

            for (int i = 0; i < Message.Length; i++)
            {
                Message[i] = (byte)_stream.BaseStream.ReadByte();
            }

            Message[0] = (byte)(Header.Size & 0xFF);
            Message[1] = (byte)((Header.Size >> 8) & 0xFF);

            return Message.Length;
        }

        #endregion

        #region GetStream

        /// <summary>
        /// Gets stream of packet
        /// </summary>
        internal dynamic GetStream
        {
            get { return this._stream; }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (_stream != null)
                _stream.Close();
        }

        #endregion

        #endregion
    }
}
