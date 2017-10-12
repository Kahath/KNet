/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System.Net.Sockets;

namespace KNetFramework.Managers.Interface
{
	public interface IBufferManager : IManager
	{
		bool SetBuffer(SocketAsyncEventArgs e);
		void FreeBuffer(SocketAsyncEventArgs e);
	}
}
