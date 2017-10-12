/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using System;
using System.Collections.Concurrent;

namespace ServerFramework.Helpers
{
	public class ObjectPool<T>
	{
		#region Fields

		private ConcurrentStack<T> _stackPool;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ObjectPool{T}"/> type.
		/// </summary>
		/// <param name="capacity">Capacity of pool.</param>
		public ObjectPool(int capacity)
		{
			_stackPool = new ConcurrentStack<T>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets stack count.
		/// </summary>
		public int Count
		{
			get { return _stackPool.Count; }
		}

		#endregion

		#region Methods

		#region Push

		/// <summary>
		/// Pushes item onto stack.
		/// </summary>
		/// <param name="item">object.</param>
		public void Push(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			_stackPool.Push(item);
		}

		#endregion

		#region Pop

		/// <summary>
		/// Pops object from stack.
		/// </summary>
		/// <returns>object.</returns>
		public T Pop()
		{
			_stackPool.TryPop(out T retVal);

			return retVal;
		}

		#endregion

		#endregion
	}
}
