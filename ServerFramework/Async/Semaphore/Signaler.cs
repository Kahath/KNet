/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServerFramework.Async.Semaphore
{
	public class Signaler
	{
		#region Fields

		private readonly Task _green = Task.FromResult(true);
		private readonly object _lock = new object();
		private ConcurrentQueue<TaskCompletionSource<bool>> _waitingQueue;
		private bool _isGreen;

		#endregion

		#region Properties

		private Task Green
		{
			get { return _green; }
		}

		private object Lock
		{
			get { return _lock; }
		}

		internal ConcurrentQueue<TaskCompletionSource<bool>> WaitingQueue
		{
			get { return _waitingQueue; }
		}

		private bool IsGreen
		{
			get { return _isGreen; }
			set { _isGreen = value; }
		}

		#endregion

		#region Constructors

		public Signaler()
		{
			_waitingQueue = new ConcurrentQueue<TaskCompletionSource<bool>>();
			IsGreen = true;
		}

		#endregion

		#region Methods

		#region WaitGreen

		public Task WaitGreen()
		{
			Task retVal;

			lock(Lock)
			{
				if (IsGreen)
				{
					retVal = Green;
					IsGreen = false;
				}
				else
				{
					TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
					WaitingQueue.Enqueue(tcs);
					retVal = tcs.Task;
				}
			}

			return retVal;
		}

		#endregion

		#region SetGreen

		public void SetGreen()
		{
			TaskCompletionSource<bool> red = null;

			lock (Lock)
			{
				if (WaitingQueue.Count > 0)
				{
					WaitingQueue.TryDequeue(out red);
				}
				else if (!IsGreen)
				{
					IsGreen = true;
				}
			}

			if (red != null)
				red.SetResult(true);
		}

		#endregion

		#endregion
	}
}
