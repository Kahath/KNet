/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KNetFramework.Managers.Interface
{
	public interface ISessionManager : IManager
	{
		#region Properties

		ConcurrentDictionary<int, Client> Clients { get; set; }
		//Stack<int> FreeSessionIDPool { get; set; }
		int ClientsCount { get; }

		#endregion

		#region Methods

		#region RemoveClient

		Client RemoveClient(int id);

		#endregion

		#region GetClient

		Client GetClient(Func<Client, bool> func);

		Client GetClient(int sessionId);

		#endregion

		#region GetClients

		IEnumerable<Client> GetClients(Func<Client, bool> func = null);

		#endregion;

		#region AddClient

		int AddClient(Client c);

		#endregion

		#region ForEachParallel

		ParallelLoopResult ForEachParallel(Action<Client> body);

		#endregion

		#region ForEach

		void ForEach(Action<Client> body);

		#endregion

		#endregion
	}
}
