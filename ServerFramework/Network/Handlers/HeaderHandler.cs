using ServerFramework.Network.Packets;
using System;
using System.Net.Sockets;

namespace ServerFramework.Network.Handlers
{
    public class HeaderHandler
    {
        #region Methods

        /// <summary>
        /// Handles message header. If received bytes length is lesser than 
        /// header length, multiple method calls are required.
        /// </summary>
        /// <param name="e">SocketAsyncEventArgs object</param>
        /// <param name="token">SocketAsyncEventArgs user token</param>
        /// <param name="remainingBytesToProcess">bytes transfered in receiveCallback</param>
        /// <returns></returns>
        public int HandleHeader(SocketAsyncEventArgs e,
            UserToken token, int remainingBytesToProcess)
        {
            if (token.HeaderBytesDoneCount == 0)
                token.Header = new byte[token.HeaderLength];

            if (remainingBytesToProcess >= token.HeaderLength -
                token.HeaderBytesDoneCount)
            {
                Buffer.BlockCopy(e.Buffer,
                    token.MessageOffset -
                    token.HeaderLength +
                    token.HeaderBytesDoneCount,
                    token.Header,
                    token.HeaderBytesDoneCount,
                    token.HeaderLength -
                    token.HeaderBytesDoneCount);

                remainingBytesToProcess = (remainingBytesToProcess - token.HeaderLength) +
                    token.HeaderBytesDoneCount;

                token.HeaderBytesDoneThisOp = token.HeaderLength -
                    token.HeaderBytesDoneCount;

                token.HeaderBytesDoneCount = token.HeaderLength;

                token.MessageLength = BitConverter.ToInt16(
                    token.Header, 0);

                token.PrepareReceive();

                token.Packet.Header = new PacketHeader
                {
                    Size = BitConverter.ToUInt16(token.Header, 0),
                    Opcode = BitConverter.ToUInt16(token.Header, 2)
                };

                token.HeaderReady = true;
            }
            else
            {
                Buffer.BlockCopy(e.Buffer,
                    token.MessageOffset -
                    token.HeaderLength +
                    token.HeaderBytesDoneCount,
                    token.Header,
                    token.HeaderBytesDoneCount,
                    remainingBytesToProcess);

                token.HeaderBytesDoneThisOp = remainingBytesToProcess;
                token.HeaderBytesDoneCount += remainingBytesToProcess;
                remainingBytesToProcess = 0;
            }

            if (remainingBytesToProcess == 0)
            {
                token.MessageOffset -= token.HeaderBytesDoneThisOp;
                token.HeaderBytesDoneThisOp = 0;
            }

            return remainingBytesToProcess;
        }

        #endregion       
    }
}
