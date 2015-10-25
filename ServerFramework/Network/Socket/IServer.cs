/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	public interface IServer : IDisposable
	{
		#region Events

		event ServerEventHandler CloseClientSocket;
		event ServerEventHandler Connect;

		#endregion

		#region Methods

		void Init();
		void Send(SocketAsyncEventArgs e);

		#endregion
	}
}
