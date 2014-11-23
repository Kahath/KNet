using ServerFramework.Constants.Misc;
using ServerFramework.Constants.NetMessage;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;

namespace ServerFramework.Constants.Entities.Session
{
    public class Client
    {
        #region Fields

        private Saea _saea;

        #endregion

        #region Constructors

        public Client(Saea saea)
        {

        }

        #endregion

        #region Properties

        public Saea Saea
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
            item.PrepareSend();
            Log.Message(LogType.Debug, "Packet Content {0}", BitConverter.ToString(item.Packet.Message));
            Manager.Server.Send(this.Saea.Sender);
        }

        #endregion

        #region PrepareSend
		 
        public UserToken PrepareSend(SMSG opcode)
        {
            //sendResetEvent.WaitOne();
            this.Saea.SendResetEvent.WaitOne();
            UserToken token = (UserToken)Saea.Sender.UserToken;
            token.PrepareWrite((ushort)opcode);
            return token;
        }

        #endregion

        #endregion  
    }
}
