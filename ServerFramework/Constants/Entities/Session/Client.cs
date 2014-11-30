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

using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
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
