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

using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerFramework.Managers.Base
{
	public abstract class BufferManagerBase<T> : ManagerBase<T> where T : class
	{
		#region Fields

		private int _bufferBytesAllocatedForEachSaea;
		private int _totalBytesInBufferBlock;
		private int _currentIndex;
		private byte[] _bufferBlock;
		private Stack<int> _freeIndexPool;

		#endregion

		#region Properties

		protected int BufferBytesAllocatedForEachSaea
		{
			get { return _bufferBytesAllocatedForEachSaea; }
			set { _bufferBytesAllocatedForEachSaea = value; }
		}

		public int TotalBytesInBufferBlock
		{
			get { return _totalBytesInBufferBlock; }
			protected set { _totalBytesInBufferBlock = value; }
		}

		protected int CurrentIndex
		{
			get { return _currentIndex; }
			set { _currentIndex = value; }
		}

		protected byte[] BufferBlock
		{
			get { return _bufferBlock; }
			set { _bufferBlock = value; }
		}

		protected Stack<int> FreeIndexPool
		{
			get { return _freeIndexPool; }
			set { _freeIndexPool = value; }
		}

		#endregion

		#region Constructors

		protected BufferManagerBase(int totalBytes, int totalBytesInEachSaeaObject)
		{
		}

		#endregion

		#region Methods

		internal abstract bool SetBuffer(SocketAsyncEventArgs e);
		internal abstract void FreeBuffer(SocketAsyncEventArgs e);

		#endregion
	}
}
