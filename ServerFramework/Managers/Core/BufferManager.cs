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

using ServerFramework.Constants.Misc;
using ServerFramework.Managers.Base;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerFramework.Managers.Core
{
	public sealed class BufferManager : BufferManagerBase<BufferManager>
	{
		#region Constructors

		BufferManager(int totalBytes, int totalBytesInEachSaeaObject)
			: base(totalBytes, totalBytesInEachSaeaObject)
		{
			TotalBytesInBufferBlock = totalBytes;
			CurrentIndex = 0;
			BufferBytesAllocatedForEachSaea = totalBytesInEachSaeaObject;
			FreeIndexPool = new Stack<int>();

			Init();
		}

		#endregion

		#region Methods

		#region Init

		internal override void Init()
		{
			BufferBlock = new byte[TotalBytesInBufferBlock];
			Manager.LogMgr.Log(LogType.Normal, "Buffer allocated size: {0}KB", TotalBytesInBufferBlock / 1024);
		}

		#endregion

		#region SetBuffer

		internal override bool SetBuffer(SocketAsyncEventArgs e)
		{
			if (FreeIndexPool.Count > 0)
			{
				e.SetBuffer(BufferBlock, FreeIndexPool.Pop(),
					BufferBytesAllocatedForEachSaea);
			}
			else
			{
				if ((TotalBytesInBufferBlock - BufferBytesAllocatedForEachSaea) <
					CurrentIndex)
					return false;

				e.SetBuffer(BufferBlock, CurrentIndex,
					BufferBytesAllocatedForEachSaea);

				CurrentIndex += BufferBytesAllocatedForEachSaea;
			}

			return true;
		}

		#endregion

		#region FreeBuffer

		internal override void FreeBuffer(SocketAsyncEventArgs e)
		{
			FreeIndexPool.Push(e.Offset);
			e.SetBuffer(null, 0, 0);
		}

		#endregion

		#endregion
	}
}
