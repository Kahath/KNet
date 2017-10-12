/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Managers.Base;
using ServerFramework.Managers.Interface;
using ServerFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerFramework.Managers.Core
{
	public class SessionManager : ManagerBase<SessionManager, ISessionManager>
	{
		#region Properties

		internal ConcurrentDictionary<int, Client> Clients
		{
			get { return Instance.Clients; }
			set { Instance.Clients = value; }
		}

		//internal Stack<int> FreeSessionIDPool
		//{
		//	get { return Instance.FreeSessionIDPool; }
		//	set { Instance.FreeSessionIDPool = value; }
		//}

		public int ClientsCount
		{
			get { return Instance.ClientsCount; }
		}

		#endregion

		#region Methods

		#region RemoveClient

		/// <summary>
		/// Removes client from collection.
		/// </summary>
		/// <param name="id">ID of client to remove.</param>
		/// <returns>Instance of removed <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.</returns>
		internal Client RemoveClient(int id)
		{
			return Instance.RemoveClient(id);
		}

		#endregion

		#region GetClient

		/// <summary>
		/// Gets client from collection.
		/// </summary>
		/// <param name="func">Function as filter for client.</param>
		/// <returns>Instance of filtered <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.</returns>
		public Client GetClient(Func<Client, bool> func)
		{
			return Instance.GetClient(func);
		}

		/// <summary>
		/// Gets client from collection by sessionID.
		/// </summary>
		/// <param name="sessionId">Session ID.</param>
		/// <returns>Instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.</returns>
		public Client GetClient(int sessionId)
		{
			return Instance.GetClient(sessionId);
		}

		#endregion

		#region GetClients

		/// <summary>
		/// Gets collection of clients
		/// </summary>
		/// <param name="func">function as filter</param>
		/// <returns>Collection of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.</returns>
		public IEnumerable<Client> GetClients(Func<Client, bool> func = null)
		{
			return Instance.GetClients(func);
		}

		#endregion

		#region AddClient

		/// <summary>
		/// Adds client to session.
		/// </summary>
		/// <param name="c">Instance of <see cref="ServerFramework.Constants.Entities.Session.Client"/> type.</param>
		/// <returns>Session ID.</returns>
		internal int AddClient(Client c)
		{
			return Instance.AddClient(c);
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
			return Instance.ForEachParallel(body);
		}

		#endregion

		#region ForEach

		/// <summary>
		/// Executes action for each Client in session.
		/// </summary>
		/// <param name="body">Action method.</param>
		public void ForEach(Action<Client> body)
		{
			Instance.ForEach(body);
		}

		#endregion

		#endregion

	}
}
