/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using ServerFramework.Configuration.Helpers;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	public class Server : Dependency<IServer>
	{
		#region Constructors

		public Server(SocketListenerSettings socketSettings)
			: base(ResolveType.Singleton, socketSettings)
		{

		}

		#endregion

		#region Events

		public event ServerEventHandler CloseClientSocket
		{
			add { Instance.CloseClientSocket += value; }
			remove { Instance.CloseClientSocket -= value; }
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

		#endregion
	}
}
