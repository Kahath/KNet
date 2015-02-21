﻿/*
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

using System.Net;

namespace ServerFramework.Configuration
{
    public class SocketListenerSettings
    {
        #region Fields

        private int _maxConnections;
        private int _numberOfSaeaForRecSend;
        private int _backlog;
        private int _maxSimultaneousAcceptOps;
        private int _bufferSize;
        private int _headerLength;
        private IPEndPoint _localEndPoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new object with settings for Buffer and Socket.
        /// </summary>
        /// <param name="maxConnections">Maximum number of connections allowed on server</param>
        /// <param name="backlog">Number of queued connections if maximum number is surpassed</param>
        /// <param name="maxAcceptOps">Maximum number of SocketAsyncEventArgs objects for accepting connections</param>
        /// <param name="bufferSize">Buffer size for each SocketAsyncEventArgs object</param>
        /// <param name="headerLength">Length of message header</param>
        /// <param name="localEndPoint">IP address and port of listening</param>
        internal SocketListenerSettings(int maxConnections, int backlog,
            int maxAcceptOps, int bufferSize,
            int headerLength, IPEndPoint localEndPoint)
        {
            this._maxConnections = maxConnections;
            this._numberOfSaeaForRecSend = maxConnections;
            this._backlog = backlog;
            this._maxSimultaneousAcceptOps = maxAcceptOps;
            this._bufferSize = bufferSize;
            this._headerLength = headerLength;
            this._localEndPoint = localEndPoint;
        }

        #endregion

        #region Properties

        public int MaxConnections
        {
            get { return this._maxConnections; }
        }

        public int NumberOfSaeaForRecSend
        {
            get { return this._numberOfSaeaForRecSend; }
        }

        public int Backlog
        {
            get { return this._backlog; }
        }

        public int MaxAcceptOps
        {
            get { return this._maxSimultaneousAcceptOps; }
        }

        public int HeaderLength
        {
            get { return this._headerLength; }
        }

        public int BufferSize
        {
            get { return this._bufferSize; }
        }

        public IPEndPoint LocalEndPoint
        {
            get { return this._localEndPoint; }
        }    

        #endregion   
    }
}
