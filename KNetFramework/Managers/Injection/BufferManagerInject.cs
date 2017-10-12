/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Enums;
using KNetFramework.Managers.Interface;
using System.Collections.Generic;
using System.Net.Sockets;

namespace KNetFramework.Managers.Injection
{
	public sealed class BufferManagerInject : IBufferManager
	{
		#region Fields

		private int _bufferBytesAllocatedForEachSaea;
		private int _totalBytesInBufferBlock;
		private int _currentIndex;
		private byte[] _bufferBlock;
		private Stack<int> _freeIndexPool;

		#endregion

		#region Properties

		private int BufferBytesAllocatedForEachSaea
		{
			get { return _bufferBytesAllocatedForEachSaea; }
		}

		public int TotalBytesInBufferBlock
		{
			get { return _totalBytesInBufferBlock; }
		}

		private int CurrentIndex
		{
			get { return _currentIndex; }
		}

		private byte[] BufferBlock
		{
			get { return _bufferBlock; }
		}

		private Stack<int> FreeIndexPool
		{
			get { return _freeIndexPool; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="KNetFramework.Managers.Core.BufferManager"/> type.
		/// </summary>
		/// <param name="totalBytes">Total bytes in buffer</param>
		/// <param name="totalBytesInEachSaeaObject">Total bytes for each connetion socket.</param>
		BufferManagerInject(int totalBytes, int totalBytesInEachSaeaObject)
		{
			_totalBytesInBufferBlock = totalBytes;
			_currentIndex = 0;
			_bufferBytesAllocatedForEachSaea = totalBytesInEachSaeaObject;
			_freeIndexPool = new Stack<int>();

			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises BufferManager.
		/// </summary>
		public void Init()
		{
			_bufferBlock = new byte[TotalBytesInBufferBlock];
			Manager.LogManager.Log(LogTypes.Normal, $"Buffer allocated size: {TotalBytesInBufferBlock / 1024}KB");
		}

		#endregion

		#region SetBuffer

		/// <summary>
		/// Sets buffer for Socket.
		/// </summary>
		/// <param name="e">Instance of <see cref="SocketAsyncEventArgs"/> type.</param>
		/// <returns>True if set.</returns>
		public bool SetBuffer(SocketAsyncEventArgs e)
		{
			bool retVal = false;

			if (FreeIndexPool.Count > 0)
			{
				e.SetBuffer(BufferBlock, FreeIndexPool.Pop(), BufferBytesAllocatedForEachSaea);
			}
			else
			{
				if ((TotalBytesInBufferBlock - BufferBytesAllocatedForEachSaea) >= CurrentIndex)
				{
					e.SetBuffer(BufferBlock, CurrentIndex, BufferBytesAllocatedForEachSaea);

					_currentIndex += BufferBytesAllocatedForEachSaea;
					retVal = true;
				}
			}

			return retVal;
		}

		#endregion

		#region FreeBuffer

		/// <summary>
		/// Sets buffer for socket to null
		/// </summary>
		/// <param name="e">>Instance of <see cref="SocketAsyncEventArgs"/> type.</param>
		public void FreeBuffer(SocketAsyncEventArgs e)
		{
			FreeIndexPool.Push(e.Offset);
			e.SetBuffer(null, 0, 0);
		}

		#endregion

		#endregion
	}
}
