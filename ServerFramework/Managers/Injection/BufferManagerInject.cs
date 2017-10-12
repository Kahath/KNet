/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Enums;
using ServerFramework.Managers.Interface;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerFramework.Managers.Injection
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
			set { _bufferBytesAllocatedForEachSaea = value; }
		}

		public int TotalBytesInBufferBlock
		{
			get { return _totalBytesInBufferBlock; }
			private set { _totalBytesInBufferBlock = value; }
		}

		private int CurrentIndex
		{
			get { return _currentIndex; }
			set { _currentIndex = value; }
		}

		private byte[] BufferBlock
		{
			get { return _bufferBlock; }
			set { _bufferBlock = value; }
		}

		private Stack<int> FreeIndexPool
		{
			get { return _freeIndexPool; }
			set { _freeIndexPool = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Managers.Core.BufferManager"/> type.
		/// </summary>
		/// <param name="totalBytes">Total bytes in buffer</param>
		/// <param name="totalBytesInEachSaeaObject">Total bytes for each connetion socket.</param>
		BufferManagerInject(int totalBytes, int totalBytesInEachSaeaObject)
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

		/// <summary>
		/// Initialises BufferManager.
		/// </summary>
		public void Init()
		{
			BufferBlock = new byte[TotalBytesInBufferBlock];
			Manager.LogMgr.Log(LogTypes.Normal, $"Buffer allocated size: {TotalBytesInBufferBlock / 1024}KB");
		}

		#endregion

		#region SetBuffer

		/// <summary>
		/// Sets buffer for Socket.
		/// </summary>
		/// <param name="e">Instance of <see cref="System.Net.Sockets.SocketAsyncEventArgs"/> type.</param>
		/// <returns>True if set.</returns>
		public bool SetBuffer(SocketAsyncEventArgs e)
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

		/// <summary>
		/// Sets buffer for socket to null
		/// </summary>
		/// <param name="e">>Instance of <see cref="System.Net.Sockets.SocketAsyncEventArgs"/> type.</param>
		public void FreeBuffer(SocketAsyncEventArgs e)
		{
			FreeIndexPool.Push(e.Offset);
			e.SetBuffer(null, 0, 0);
		}

		#endregion

		#endregion
	}
}
