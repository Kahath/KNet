/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Managers.Base;
using ServerFramework.Managers.Interface;
using System.Net.Sockets;

namespace ServerFramework.Managers.Core
{
	public class BufferManager : ManagerBase<BufferManager, IBufferManager>
	{
		#region Methods

		#region SetBuffer

		internal bool SetBuffer(SocketAsyncEventArgs e)
		{
			return Instance.SetBuffer(e);
		}

		#endregion

		#region FreeBuffer

		internal void FreeBuffer(SocketAsyncEventArgs e)
		{
			Instance.FreeBuffer(e);
		}

		#endregion

		#endregion
	}
}
