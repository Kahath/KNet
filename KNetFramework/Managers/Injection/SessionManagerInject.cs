/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Managers.Interface;
using KNetFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KNetFramework.Managers.Injection
{
	public sealed class SessionManagerInject : ISessionManager
	{
		#region Fields

		private ConcurrentDictionary<int, Client> _clients;
		//private Stack<int> _freeSessionIDPool;
		private int _sessionId = 0;

		#endregion

		#region Properties

		public ConcurrentDictionary<int, Client> Clients
		{
			get { return _clients; }
			set { _clients = value; }
		}

		//public Stack<int> FreeSessionIDPool
		//{
		//	get { return _freeSessionIDPool; }
		//	set { _freeSessionIDPool = value; }
		//}

		public int ClientsCount
		{
			get { return _clients.Count; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="Core.SessionManager"/> type.
		/// </summary>
		SessionManagerInject()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises SessionManager.
		/// </summary>
		public void Init()
		{
			_clients = new ConcurrentDictionary<int, Client>();
			//FreeSessionIDPool = new Stack<int>();
		}

		#endregion

		#region RemoveClient

		/// <summary>
		/// Removes client from collection.
		/// </summary>
		/// <param name="id">ID of client to remove.</param>
		/// <returns>Instance of removed <see cref="KNetFramework.Constants.Entities.Session.Client"/> type.</returns>
		public Client RemoveClient(int id)
		{
			Clients.TryRemove(id, out Client client);

			return client;
		}

		#endregion

		#region GetClient

		/// <summary>
		/// Gets client from collection.
		/// </summary>
		/// <param name="func">Function as filter for client.</param>
		/// <returns>Instance of filtered <see cref="KNetFramework.Constants.Entities.Session.Client"/> type.</returns>
		public Client GetClient(Func<Client, bool> func)
		{
			Client client = null;
			client = Clients.Values.FirstOrDefault(func);

			return client;
		}

		/// <summary>
		/// Gets client from collection by sessionID.
		/// </summary>
		/// <param name="sessionId">Session ID.</param>
		/// <returns>Instance of <see cref="KNetFramework.Constants.Entities.Session.Client"/> type.</returns>
		public Client GetClient(int sessionId)
		{
			Clients.TryGetValue(sessionId, out Client c);

			return c;
		}

		#endregion

		#region GetClients

		/// <summary>
		/// Gets collection of clients
		/// </summary>
		/// <param name="func">function as filter</param>
		/// <returns>Collection of <see cref="KNetFramework.Constants.Entities.Session.Client"/> type.</returns>
		public IEnumerable<Client> GetClients(Func<Client, bool> func = null)
		{
			IEnumerable<Client> clients = Enumerable.Empty<Client>();

			if (func != null)
			{
				clients = Clients.Values.Where(func);
			}
			else
			{
				clients = Clients.Values;
			}

			return clients;
		}

		#endregion

		#region AddClient

		/// <summary>
		/// Adds client to session.
		/// </summary>
		/// <param name="c">Instance of <see cref="KNetFramework.Constants.Entities.Session.Client"/> type.</param>
		/// <returns>Session ID.</returns>
		public int AddClient(Client c)
		{
			int id = 0;
			id = Interlocked.Increment(ref _sessionId);

			if (!Clients.TryAdd(id, c))
				id = 0;

			return id;
		}

		#endregion

		#region ForEachParallel

		/// <summary>
		/// Executes action for each Client in session parallel.
		/// </summary>
		/// <param name="body">Action method</param>
		/// <returns>Instance of <see cref="System.Threading.Tasks.ParallelLoopResult"/> type.</returns>
		public ParallelLoopResult ForEachParallel(Action<Client> body)
		{
			ParallelLoopResult retVal;

			retVal = Parallel.ForEach(Clients.Values, body);

			return retVal;
		}

		#endregion

		#region ForEach

		/// <summary>
		/// Executes action for each Client in session.
		/// </summary>
		/// <param name="body">Action method.</param>
		public void ForEach(Action<Client> body)
		{
			foreach (Client client in Clients.Values)
				body(client);
		}

		#endregion

		#endregion
	}
}
