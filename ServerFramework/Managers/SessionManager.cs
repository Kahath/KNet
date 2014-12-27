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

using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Singleton;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServerFramework.Managers
{
    public sealed class SessionManager : SingletonBase<SessionManager>
    {
        #region Fields

        private ConcurrentDictionary<int, Client> _clients;
        private Stack<int> _freeSessionIDPool;
        private int _sessionId = 0;

        #endregion

        #region Properties

        internal ConcurrentDictionary<int, Client> Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }

        internal Stack<int> FreeSessionIDPool
        {
            get { return _freeSessionIDPool; }
            set { _freeSessionIDPool = value; }
        }

        public int ClientsCount
        {
            get { return _clients.Count; }
        }

        #endregion

        #region Constructors

        SessionManager()
        {
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal override void Init()
        {
            Clients = new ConcurrentDictionary<int, Client>();
            FreeSessionIDPool = new Stack<int>();

            base.Init();
        }

        #endregion

        #region RemoveClient

        internal Client RemoveClient(int id)
        {
            Client client = null;

            if( Clients.TryRemove(id, out client))
                FreeSessionIDPool.Push(id);

            return client;
        }

        #endregion

        #region GetClientBySessionId

        public Client GetClientBySessionId(int sessionId)
        {
            Client c = null;
            
            if(Clients.TryGetValue(sessionId, out c))
                return c;

            return null;
        }

        #endregion

        #region AddClient

        internal int AddClient(Client c)
        {
            int id;

            id = FreeSessionIDPool.Count > 0 ? FreeSessionIDPool.Pop() :
                Interlocked.Increment(ref _sessionId);

            Clients.TryAdd(id, c);
            LogManager.Log(LogType.Debug, "New session");

            return id;
        }

        #endregion

        #region GetClients

        public IEnumerable<Client> GetClients()
        {
            foreach (KeyValuePair<int, Client> client in Clients)
                yield return client.Value;
        }

        #endregion

        #endregion
    }
}
