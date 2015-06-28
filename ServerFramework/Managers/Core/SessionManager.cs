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
using ServerFramework.Managers.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerFramework.Managers.Core
{
	public sealed class SessionManager : ManagerBase<SessionManager>
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
		}

		#endregion

		#region RemoveClient

		internal Client RemoveClient(int id)
		{
			Client client = null;

			if (Clients.TryRemove(id, out client))
				FreeSessionIDPool.Push(id);

			return client;
		}

		#endregion

		#region GetClient

		public Client GetClient(Func<Client, bool> func)
		{
			Client client = null;
			client = Clients.Values.FirstOrDefault(func);

			return client;
		}

		public Client GetClientBySession(int sessionId)
		{
			Client c = null;

			Clients.TryGetValue(sessionId, out c);

			return c;
		}

		#endregion

		#region GetClients

		public IEnumerable<Client> GetClients(Func<Client, bool> func = null)
		{
			IEnumerable<Client> clients = null;

			if (func != null)
				clients = Clients.Values.Where(func);
			else
				clients = Clients.Values;

			return clients;
		}

		#endregion

		#region AddClient

		internal int AddClient(Client c)
		{
			int id = 0;

			id = FreeSessionIDPool.Count > 0 ? FreeSessionIDPool.Pop() 
				: Interlocked.Increment(ref _sessionId);

			if (Clients.TryAdd(id, c))
			{
				Manager.LogMgr.Log(LogType.Info, "New session");
			}
			else
			{
				if (id > 0)
					FreeSessionIDPool.Push(id);

				id = 0;
			}

			return id;
		}

		#endregion

		#region ForEachParallel

		public ParallelLoopResult ForEachParallel(Action<Client> body)
		{
			ParallelLoopResult retVal;

			retVal = Parallel.ForEach(Clients.Values, body);

			return retVal;
		}

		#endregion

		#region ForEach

		public void ForEach(Action<Client> body)
		{
			foreach (Client client in Clients.Values)
			{
				body(client);
			}
		}

		#endregion

		#endregion
	}
}
