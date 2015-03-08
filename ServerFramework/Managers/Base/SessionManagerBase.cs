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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerFramework.Managers.Base
{
	public abstract class SessionManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private ConcurrentDictionary<int, Client> _clients;
		private Stack<int> _freeSessionIDPool;
		protected int _sessionId = 0;

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

		#region Methods

		internal abstract Client RemoveClient(int id);
		public abstract Client GetClient(Func<Client, bool> func);
		public abstract Client GetClientBySessionId(int sessionId);
		public abstract IEnumerable<Client> GetClients(Func<Client, bool> func = null);
		internal abstract int AddClient(Client c);

		#endregion
	}
}
