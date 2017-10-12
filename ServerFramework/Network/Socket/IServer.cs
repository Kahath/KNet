/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.Net.Sockets;

namespace ServerFramework.Network.Socket
{
	public interface IServer : IDisposable
	{
		#region Properties

		bool IsRunning { get; }

		#endregion

		#region Events

		event ServerEventHandler ClosingClientSocket;
		event ServerEventHandler Connect;

		#endregion

		#region Methods

		void Init();
		void Send(SocketAsyncEventArgs e);
		void Quit();

		#endregion
	}
}
