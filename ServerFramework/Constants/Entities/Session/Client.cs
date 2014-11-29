using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;

namespace ServerFramework.Constants.Entities.Session
{
    public sealed class Client
    {
        #region Fields

        private Saea _saea;

        #endregion

        #region Constructors

        internal Client(Saea saea)
        {
            Saea = saea;
        }

        #endregion

        #region Events

        public event PacketSendEventHandler BeforePacketSend;

        #endregion

        #region Properties

        internal Saea Saea
        {
            get { return _saea; }
            set { _saea = value; }
        }

        #endregion

        #region Methods

        #region GetIP

        public string GetIP()
        {
            return Saea.Receiver.AcceptSocket.RemoteEndPoint.ToString();
        }

        #endregion

        #region Send
		
        public void Send(UserToken item)
        {
            if (BeforePacketSend != null)
                BeforePacketSend(item.Packet, new EventArgs());

            item.PrepareSend();
            LogManager.Log(LogType.Debug, "Packet Content {0}", BitConverter.ToString(item.Packet.Message));
            Server.GetInstance().Send(this.Saea.Sender);
        }

        #endregion

        #region PrepareSend
		 
        public UserToken PrepareSend(ushort opcode)
        {
            this.Saea.SendResetEvent.WaitOne();
            UserToken token = (UserToken)Saea.Sender.UserToken;
            token.PrepareWrite(opcode);
            return token;
        }

        #endregion

        #endregion  
    }
}
