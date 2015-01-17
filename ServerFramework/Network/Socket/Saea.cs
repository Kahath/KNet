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

using ServerFramework.Network.Packets;
using System.Net.Sockets;
using System.Threading;

namespace ServerFramework.Network.Socket
{
    internal sealed class Saea
    {
        #region Fields

        private SocketAsyncEventArgs _sender;
        private SocketAsyncEventArgs _receiver;
        private AutoResetEvent _sendResetEvent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates object with SocketAsyncEventArgs objects
        /// for sending and receiving data
        /// </summary>
        internal Saea()
        {
            _sendResetEvent = new AutoResetEvent(true);
        }

        #endregion

        #region Properties

        internal SocketAsyncEventArgs Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        internal SocketAsyncEventArgs Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        internal AutoResetEvent SendResetEvent 
        {
            get { return _sendResetEvent; }
            set { _sendResetEvent = value; }
        }

        internal System.Net.Sockets.Socket AcceptSocket
        {
            set 
            { 
                Sender.AcceptSocket = value;
                Receiver.AcceptSocket = value;
            }
        }

        #endregion

        #region Methods

        #region AssignId

        public void AssignId(int id)
        {
            ((UserToken)Sender.UserToken).SessionId = id;
            ((UserToken)Receiver.UserToken).SessionId = id;
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes both SocketAsyncEventArgs objects
        /// </summary>
        internal void Close()
        {
            this.Sender.AcceptSocket.Close();
            this.Receiver.AcceptSocket.Close();
        }

        #endregion

        #region Shutdown

        /// <summary>
        /// Shutdown both SocketAsyncEventArgs objects
        /// </summary>
        /// <param name="how"></param>
        internal void Shutdown(SocketShutdown how)
        {
            try
            {
                this.Sender.AcceptSocket.Shutdown(how);
                this.Receiver.AcceptSocket.Shutdown(how);
                this._sendResetEvent.Set();
            }
            catch (SocketException) { }
        }

        #endregion
        
        #endregion
    }
}
