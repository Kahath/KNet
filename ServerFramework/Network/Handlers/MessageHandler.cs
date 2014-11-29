using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using System;
using System.Net.Sockets;

namespace ServerFramework.Network.Handlers
{
    public class MessageHandler
    {
        #region Methods

        /// <summary>
        /// Handles message.  If received bytes length is lesser than 
        /// message length, multiple method calls are required. 
        /// </summary>
        /// <param name="e">SocketAsyncEventArgs object</param>
        /// <param name="token">SocketAsyncEventArgs UserToken</param>
        /// <param name="remainingBytesToProcess">bytes transfered in receive callback</param>
        /// <returns></returns>
        public int HandleMessage(SocketAsyncEventArgs e,
            UserToken token,
            int remainingBytesToProcess)
        {
            if (token.MessageBytesDoneCount == 0)
                token.Packet.Message = new
                    byte[token.MessageLength];

            if (token.MessageLength == 0)
            {
                token.Packet.SessionId = token.SessionId;
                token.Packet.PrepareRead();

                token.PacketReady = true;
                LogManager.Log(LogType.Debug, "Session Id: {0} Message handled", token.SessionId);

                return remainingBytesToProcess;
            }

            LogManager.Log(LogType.Debug, "Handling message");

            if ((remainingBytesToProcess +
                token.MessageBytesDoneCount) >=
                token.MessageLength)
            {
                Buffer.BlockCopy(e.Buffer,
                    token.MessageOffset,
                    token.Packet.Message,
                    token.MessageBytesDoneCount,
                    token.MessageLength -
                    token.MessageBytesDoneCount);

                remainingBytesToProcess = (remainingBytesToProcess - token.MessageLength)
                    + token.MessageBytesDoneCount;

                token.Packet.SessionId = token.SessionId;
                token.Packet.PrepareRead();

                token.PacketReady = true;
                LogManager.Log(LogType.Debug, "Session Id: {0} Message handled", token.SessionId);
            }
            else
            {
                Buffer.BlockCopy(e.Buffer,
                    token.MessageOffset,
                    token.Packet.Message,
                    token.MessageBytesDoneCount,
                    remainingBytesToProcess);

                token.MessageOffset -= token.HeaderBytesDoneThisOp;

                token.MessageBytesDoneCount += remainingBytesToProcess;
                remainingBytesToProcess = 0;

                LogManager.Log(LogType.Debug, "Message not fully handled!");
            }
            return remainingBytesToProcess;
        }

        #endregion       
    }
}
