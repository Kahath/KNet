﻿/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System;
using System.Collections.Generic;


namespace ServerFramework.Helpers
{
	public class ObjectPool<T>
	{
		#region Fields

		private Stack<T> _stackPool;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Constants.Entities.Console.Misc.ObjectPool{T}"/> type.
		/// </summary>
		/// <param name="capacity">Capacity of pool.</param>
		public ObjectPool(int capacity)
		{
			_stackPool = new Stack<T>(capacity);
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

			lock (_stackPool)
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
			lock (_stackPool)
				return _stackPool.Pop();
		}

		#endregion

		#endregion
	}
}
