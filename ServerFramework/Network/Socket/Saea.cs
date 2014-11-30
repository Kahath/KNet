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

        #endregion

        #region Methods

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
