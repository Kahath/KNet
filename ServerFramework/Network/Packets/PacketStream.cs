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
    internal  class PacketStream : IDisposable
    {
        #region Fields

        private BinaryReader _reader;
        private BinaryWriter _writer;

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

        public PacketStream(Encoding encoder, byte[] data = null)
        {
            if (data != null)
                Reader = new BinaryReader(new MemoryStream(data), encoder);
            else
                Writer = new BinaryWriter(new MemoryStream(), encoder);
        }

        #endregion

        #region Methods

        #region Read

        /// <summary>
        /// Used for reading from packet buffer
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="count">not used</param>
        /// <returns>Generic result</returns>
        internal T Read<T>(int count = 0)
        {
            if (Reader != null)
                return Reader.Read<T>(count);

            throw new NullReferenceException("Reader cannot be null");
        }

        #endregion

        #region Write

        /// <summary>
        /// Writes value to stream buffer.
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="value">value of method type</param>
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

        internal T ReadBits<T>(int count)
        {
            int retVal = 0;

            for (int i = count - 1; i >= 0; --i)
                retVal = ReadBit() ? (1 << i) | retVal : retVal;

            return (T)Convert.ChangeType(retVal, typeof(T));
        }

        #endregion

        #region WriteBit

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

        internal void WriteBits<T>(T value, int count)
        {
            for (int i = count - 1; i >= 0; --i)
                WriteBit((bool)Convert.ChangeType(
                    (Convert.ToInt32(value) >> i) & 1, typeof(bool)));
        }

        #endregion

        #region Flush

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
        /// Readies packet for sending.
        /// </summary>
        /// <returns>Size of packet minus header size</returns>
        internal int End(out byte[] message)
        {
            Flush();
            Writer.BaseStream.Seek(0, SeekOrigin.Begin);
            message = new byte[Writer.BaseStream.Length];
            ushort size = (ushort)(message.Length - ServerConfig.HeaderLength);

            for (int i = 0; i < message.Length; i++)
            {
                message[i] = (byte)Writer.BaseStream.ReadByte();
            }

            message[0] = (byte)(size & 0xFF);
            message[1] = (byte)((size >> 8) & 0xFF);

            return message.Length;
        }

        #endregion

        #region Dispose

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
