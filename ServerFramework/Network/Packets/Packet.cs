using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
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

        /// <summary>
        /// Gets stream of packet
        /// </summary>
        internal dynamic GetStream
        {
            get { return this.stream; }
        }

        dynamic stream;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new object for reading message.
        /// </summary>
        internal Packet()
        {
            //stream = new BinaryReader(new MemoryStream());
            Header = new PacketHeader();
        }

        /// <summary>
        /// Creates new object for writing message
        /// </summary>
        /// <param name="message">opcode of message</param>
        internal Packet(ushort message)
        {
            stream = new BinaryWriter(new MemoryStream());
            Header = new PacketHeader
            {
                Size = 4,
                Opcode = message
            };

            Write<ushort>(Header.Size);
            Write<ushort>(Header.Opcode);
        }

        #endregion

        #region Reader
        /// <summary>
        /// Used for reading from packet buffer
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="count">not used</param>
        /// <returns>Byte, SByte, UInt16, Int16, UInt32, Int32,
        /// UInt64, Int64, Char, Double, Single, Boolea, Pascal String
        /// depending of method type</returns>
        internal T Read<T>(int count = 0)
        {
            if (stream is BinaryWriter)
                return default(T);

            switch(typeof(T).Name)
            {
                case "Byte":
                    return stream.ReadByte();
                case "SByte":
                    return stream.ReadSByte();
                case "UInt16":
                    return stream.ReadUInt16();
                case "Int16":
                    return stream.ReadInt16();
                case "UInt32":
                    return stream.ReadUInt32();
                case "Int32":
                    return stream.ReadInt32();
                case "UInt64":
                    return stream.ReadUInt64();
                case "Int64":
                    return stream.ReadInt64();
                case "Char":
                    return stream.ReadChar();
                case "Double":
                    return stream.ReadDouble();
                case "Single":
                    return stream.ReadSingle();
                case "Boolean":
                    return stream.ReadBoolean();
                case "String":
                    var bytes = stream.ReadBytes(stream.ReadByte());
                    return Encoding.UTF8.GetString(bytes);
                default:
                    return default(T);
            }
        }
        #endregion

        #region Writer

        /// <summary>
        /// Writes value to stream buffer.
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="value">value of method type</param>
        internal void Write<T>(T value)
        {
            if (stream is BinaryReader)
                return;

            switch(typeof(T).Name)
            {
                case "Byte":
                    stream.Write(Convert.ToByte(value));
                    break;
                case "SByte":
                    stream.Write(Convert.ToSByte(value));
                    break;
                case "UInt16":
                    stream.Write(Convert.ToUInt16(value));
                    break;
                case "Int16":
                    stream.Write(Convert.ToInt16(value));
                    break;
                case "UInt32":
                    stream.Write(Convert.ToUInt32(value));
                    break;
                case "Int32":
                    stream.Write(Convert.ToInt32(value));
                    break;
                case "UInt64":
                    stream.Write(Convert.ToUInt64(value));
                    break;
                case "Int64":
                    stream.Write(Convert.ToInt64(value));
                    break;
                case "Single":
                    stream.Write(Convert.ToSingle(value));
                    break;
                case "String":
                    var data = Encoding.UTF8.GetBytes(value as string);
                    stream.Write(Convert.ToByte(data.Length));
                    stream.Write(data);
                    break;
                case "Byte[]":
                    data = value as byte[];

                    if (data != null)
                        stream.Write(data);
                    break;
            }
        }

        /// <summary>
        /// Readies packet for sending.
        /// </summary>
        /// <returns>Size of packet minus header size</returns>
        internal int PrepareForSend()
        {
            stream.BaseStream.Seek(0, SeekOrigin.Begin);
            Message = new byte[stream.BaseStream.Length];
            Header.Size = (ushort)(Message.Length - 4);
            LogManager.Log(LogType.Debug, "Size = {0}", Header.Size);
            for (int i = 0; i < Message.Length; i++)
            {
                Message[i] = (byte)stream.BaseStream.ReadByte();
            }

            Message[0] = (byte)(Header.Size & 0xFF);
            Message[1] = (byte)((Header.Size >> 8) & 0xFF);

            return Message.Length;
        }
        #endregion

        #region Methods

        #region PrepareRead

        /// <summary>
        /// Readies buffer for reading
        /// </summary>
        internal void PrepareRead()
        {
            if (!(stream is BinaryWriter))
                stream = new BinaryReader(new MemoryStream(this.Message));
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            this.Message = null;
            if (stream != null)
                stream.Close();
        }

        #endregion

        #endregion
    }
}
