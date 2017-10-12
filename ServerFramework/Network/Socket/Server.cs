/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	public class Server : Dependency<Server, IServer>
	{
		#region Properties

		public bool IsRunning
		{
			get { return Instance.IsRunning; }
		}

		#endregion

		#region Constructors

		public Server()
			: base(ResolveType.Singleton)
		{

		}

		#endregion

		#region Events

		public event ServerEventHandler CloseClientSocket
		{
			add { Instance.ClosingClientSocket += value; }
			remove { Instance.ClosingClientSocket -= value; }
		}

		public event ServerEventHandler Connect
		{
			add { Instance.Connect += value; }
			remove { Instance.Connect -= value; }
		}

		#endregion

		#region Methods

		#region Init

		internal void Init()
		{
			Instance.Init();
		}

		#endregion

		#region Send

		internal void Send(SocketAsyncEventArgs e)
		{
			Instance.Send(e);
		}

		#endregion

		#region Quit

		public void Quit()
		{
			Instance.Quit();
		}

		#endregion

		#endregion
	}
}
