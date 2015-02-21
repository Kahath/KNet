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

using System.Text;

namespace ServerFramework.Network.Packets
{
    public sealed class UserToken
    {
        #region Fields

        private Packet _packet;

        private readonly int _bufferOffset;
        private readonly int _bufferSize;
        private int _sessionId;

        private byte[] _header;

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

        #region Properties

        internal int MessageLength
        {
            get { return _messageLength; }
            set { _messageLength = value; }
        }

        internal int MessageOffset
        {
            get { return _messageOffset; }
            set { _messageOffset = value; }
        }

        internal int HeaderLength
        {
            get { return _headerLength; }
            set { _headerLength = value; }
        }

        internal int HeaderOffset
        {
            get { return _headerOffset; }
            set { _headerOffset = value; }
        }

        internal int PermanentMessageOffset
        {
            get { return _permanentMessageOffset; }
            set { _permanentMessageOffset = value; }
        }

        internal int HeaderBytesDoneCount
        {
            get { return _headerBytesDoneCount; }
            set { _headerBytesDoneCount = value; }
        }

        internal int MessageBytesDoneCount
        {
            get { return _messageBytesDoneCount; }
            set { _messageBytesDoneCount = value; }
        }

        internal int HeaderBytesRemainingCount
        {
            get { return _headerBytesRemainingCount; }
            set { _headerBytesRemainingCount = value; }
        }

        internal int MessageBytesRemainingCount
        {
            get { return _messageBytesRemainingCount; }
            set { _messageBytesRemainingCount = value; }
        }

        internal int HeaderBytesDoneThisOp
        {
            get { return _headerBytesDoneThisOp; }
            set { _headerBytesDoneThisOp = value; }
        }

        internal bool HeaderReady
        {
            get { return _headerReady; }
            set { _headerReady = value; }
        }

        internal bool PacketReady
        {
            get { return _packetReady; }
            set { _packetReady = value; }
        }

        internal int BufferSize
        {
            get { return _bufferSize; }
        }

        internal int BufferOffset
        {
            get { return _bufferOffset; }
        }

        internal Packet Packet
        {
            get { return _packet; }
            set { _packet = value; }
        }

        public int SessionId
        {
            get { return _sessionId; }
            internal set { _sessionId = value; }
        }

        internal byte[] Header
        {
            get { return _header; }
            set { _header = value; }
        }

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

        #region Methods

        #region StartReceive

        /// <summary>
        /// Prepares packet for receiving data
        /// </summary>
        internal void StartReceive()
        {
            Packet = new Packet(Encoding.UTF8);
        }

        #endregion

        #region Finish

        public void Finish()
        {
            this.MessageBytesRemainingCount = Packet.End();
        }

        #endregion

        #region Reset

        /// <summary>
        /// Resets packet to its initial state;
        /// </summary>
        internal void Reset(int messageOffset)
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
