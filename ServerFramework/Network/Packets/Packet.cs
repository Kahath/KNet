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
using ServerFramework.Extensions;
using System;
using System.IO;
using System.Text;

namespace ServerFramework.Network.Packets
{
    public class Packet : IDisposable
    {
        #region Fields

        private PacketHeader _header;
        private byte[] _message;
        private int _sessionId;
        private Encoding _encoder;

        private byte _position = 0;
        private byte _value;

        private PacketStream _stream;

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

        internal PacketStream Stream
        {
            get { return _stream; }
        }

        private Encoding Encoder
        {
            get { return _encoder; }
            set { _encoder = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new object for reading message.
        /// </summary>
        internal Packet(Encoding encoder = null)
        {
            Header = new PacketHeader();
            Encoder = encoder ?? Encoding.UTF8;
        }

        /// <summary>
        /// Creates new object for writing message
        /// </summary>
        /// <param name="message">opcode of message</param>
        public Packet(ushort message, Encoding encoder = null)
        {
            Encoder = encoder ?? Encoding.UTF8;
            _stream = new PacketStream(Encoder);// new BinaryWriter(new MemoryStream(), Encoder);


            Header = new PacketHeader
            {
                Size = (ushort)ServerConfig.HeaderLength,
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
            _stream = new PacketStream(Encoder, this.Message);
        }

        #endregion

        #region Read

        public T Read<T>(int count = 0)
        {
            return Stream.Read<T>(count);
        }

        #endregion

        #region Write

        public void Write<T>(T value)
        {
            Stream.Write<T>(value);
        }

        #endregion

        #region BitPack

        #region WriteBit

        public void WriteBit(bool value)
        {
            WriteBits<bool>(value, 1);
        }

        #endregion

        #region WriteBits

        public void WriteBits<T>(T value, int count)
        {
            Stream.WriteBits<T>(value, count);
        }

        #endregion

        #region ReadBit

        public bool ReadBit()
        {
            return ReadBits<bool>(1);
        }

        #endregion

        #region ReadBits

        public T ReadBits<T>(int count)
        {
            return Stream.ReadBits<T>(count);
        }

        #endregion

        #endregion

        #region End

        public int End()
        {
            return Stream.End(out _message);      
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Stream.Dispose();
        }

        #endregion

        #endregion
    }
}
