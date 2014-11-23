using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Singleton;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        #endregion

        #region Constructors

        SessionManager()
        {
            Init();
        }

        #endregion

        #region Methods

        #region Init

        private void Init()
        {
            Clients = new ConcurrentDictionary<int, Client>();
            FreeSessionIDPool = new Stack<int>();
        }

        #endregion

        #region RemoveClient

        internal Client RemoveClient(int id)
        {
            Client client = null;
            lock (Clients)
            {
                if (Clients.ContainsKey(id))
                {
                    Clients.TryRemove(id, out client);
                    FreeSessionIDPool.Push(id);
                }
            }
            return client;
        }

        #endregion

        #region GetClientBySessionID

        public Client GetClientBySessionID(int id)
        {
            lock (Clients)
                return Clients.ContainsKey(id) ? Clients[id] : null;
        }

        #endregion

        #region AddClient

        internal int AddClient(Client c)
        {
            int id;
            id = FreeSessionIDPool.Count > 0 ? FreeSessionIDPool.Pop() :
                Interlocked.Increment(ref _sessionId);

            Clients[id] = c;
            Log.Message(LogType.Debug, "New session");
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
