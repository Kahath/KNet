/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServerFramework.Network.Signalers
{
	internal class Signaler
	{
		#region Fields

		private readonly static Task _completedTask = Task.FromResult(true);
		private readonly object _lock = new object();
		private ConcurrentQueue<TaskCompletionSource<bool>> _waitingQueue;
		private bool _isSignal;

		#endregion

		#region Properties

		private Task CompletedTask
		{
			get { return _completedTask; }
		}

		private object Lock
		{
			get { return _lock; }
		}

		internal ConcurrentQueue<TaskCompletionSource<bool>> WaitingQueue
		{
			get { return _waitingQueue; }
		}

		private bool IsSignal
		{
			get { return _isSignal; }
			set { _isSignal = value; }
		}

		#endregion

		#region Constructors

		public Signaler()
		{
			_waitingQueue = new ConcurrentQueue<TaskCompletionSource<bool>>();
			IsSignal = true;
		}

		#endregion

		#region Methods

		#region WaitAsync

		internal Task WaitAsync()
		{
			lock(Lock)
			{
				if (IsSignal)
				{
					IsSignal = false;
					return CompletedTask;
				}
				else
				{
					TaskCompletionSource<bool> retVal = new TaskCompletionSource<bool>();
					WaitingQueue.Enqueue(retVal);
					return retVal.Task;
				}
			}
		}

		#endregion

		#region SetAsync

		internal void SetAsync()
		{
			TaskCompletionSource<bool> toRelease = null;

			lock (Lock)
			{
				if (WaitingQueue.Count > 0)
					WaitingQueue.TryDequeue(out toRelease);
				else if (!IsSignal)
					IsSignal = true;
			}

			if (toRelease != null)
				toRelease.SetResult(true);
		}

		#endregion

		#endregion
	}
}
