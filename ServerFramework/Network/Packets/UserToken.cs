using System;

namespace ServerFramework.Network.Packets
{
    public class UserToken
    {
        #region Fields

        private Packet _packet;

        private readonly int _bufferOffset;
        private readonly int _bufferSize;
        private int _sessionId;

        public byte[] Header;

        private int _messageLength;
        private int _messageOffset;
        private int _headerLength;
        private int _headerOffset;
        private int _permanentMessageOffset;

        private int _headerBytesDoneCount = 0;
        private int _messageBytesDoneCount = 0;
        private int _headerBytesRemainingCount = 0;
        private int _messageBytesRemainingCount = 0;
        private int _headerBytesDoneThisOp = 0;

        private bool _headerReady = false;
        private bool _packetReady = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new token for SocketAsyncEventArgs.UserToken property
        /// </summary>
        /// <param name="bufferSize">Buffer size for client</param>
        /// <param name="bufferOffset">Buffer offset in large alocated buffer</param>
        /// <param name="headerLength">Length of message header</param>
        public UserToken(int bufferSize, int bufferOffset, int headerLength)
        {
            this._bufferSize = bufferSize;
            this._bufferOffset = bufferOffset;
            this.HeaderLength = headerLength;
            this.HeaderOffset = bufferOffset;
            this.MessageOffset = bufferOffset + headerLength;
            this.PermanentMessageOffset = MessageOffset;
        }

        #endregion

        #region Properties

        public int MessageLength
        {
            get { return _messageLength; }
            set { _messageLength = value; }
        }

        public int MessageOffset
        {
            get { return _messageOffset; }
            set { _messageOffset = value; }
        }

        public int HeaderLength
        {
            get { return _headerLength; }
            set { _headerLength = value; }
        }

        public int HeaderOffset
        {
            get { return _headerOffset; }
            set { _headerOffset = value; }
        }

        public int PermanentMessageOffset
        {
            get { return _permanentMessageOffset; }
            set { _permanentMessageOffset = value; }
        }

        public int HeaderBytesDoneCount
        {
            get { return _headerBytesDoneCount; }
            set { _headerBytesDoneCount = value; }
        }

        public int MessageBytesDoneCount 
        {
            get { return _messageBytesDoneCount; }
            set { _messageBytesDoneCount = value; }
        }

        public int HeaderBytesRemainingCount
        {
            get { return _headerBytesRemainingCount; }
            set { _headerBytesRemainingCount = value; }
        }

        public int MessageBytesRemainingCount
        {
            get { return _messageBytesRemainingCount; }
            set { _messageBytesRemainingCount = value; }
        }

        public int HeaderBytesDoneThisOp
        {
            get { return _headerBytesDoneThisOp; }
            set { _headerBytesDoneThisOp = value; }
        }

        public bool HeaderReady
        {
            get { return _headerReady; }
            set { _headerReady = value; }
        }

        public bool PacketReady
        {
            get { return _packetReady; }
            set { _packetReady = value; }
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        public int BufferOffset
        {
            get { return _bufferOffset; }
        }

        public Packet Packet 
        {
            get { return _packet; }
            set { _packet = value; }
        }

        public int SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        #endregion

        #region Methods

        #region AssignId

        /// <summary>
        /// Assigns session id for UserToken objects.
        /// Used to differentiate clients.
        /// </summary>
        /// <param name="id">id of this session</param>
        public void AssignId(int id)
        {
            SessionId = id;
        }

        #endregion

        #region PrepareWrite

        /// <summary>
        /// Prepares packet for writing
        /// </summary>
        /// <param name="opcode">packet opcode</param>
        public void PrepareWrite(ushort opcode)
        {
            Packet = new Packet(opcode);
        }

        #endregion

        #region PrepareSend

        /// <summary>
        /// Readies packet for sending
        /// </summary>
        public void PrepareSend()
        {
            this.MessageBytesRemainingCount = Packet.PrepareForSend();
        }

        #endregion

        #region PrepareReceive

        /// <summary>
        /// Prepares packet for receiving data
        /// </summary>
        public void PrepareReceive()
        {
            Packet = new Packet();
        }

        #endregion

        #region Write

        /// <summary>
        /// Writes data on underlying packet
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="value">value</param>
        public void Write<T>(T value)
        {
            Packet.Write<T>(value);
        }

        #endregion

        #region Read

        /// <summary>
        /// Reads data from underlying packet
        /// </summary>
        /// <typeparam name="T">type of return value</typeparam>
        /// <returns>Byte, SByte, UInt16, Int16, UInt32, Int32,
        /// UInt64, Int64, Char, Double, Single, Boolea, Pascal String
        /// depending of method type</returns>
        public T Read<T>()
        {
            return Packet.Read<T>();
        }

        #endregion

        #region Reset

        /// <summary>
        /// Resets packet to its initial state;
        /// </summary>
        public void Reset(int messageOffset)
        {
            this.Packet = null;
            this.HeaderReady = false;
            this.PacketReady = false;
            this.HeaderBytesDoneCount = 0;
            this.HeaderBytesRemainingCount = 0;
            this.MessageBytesDoneCount = 0;
            this.MessageBytesRemainingCount = 0;
            this.MessageLength = 0;
            this.MessageOffset = messageOffset;
        }

        #endregion

        #endregion
    }
}
