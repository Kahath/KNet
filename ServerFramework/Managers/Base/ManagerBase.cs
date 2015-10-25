/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Singleton;

namespace ServerFramework.Managers.Base
{
	public abstract class ManagerBase<T> : SingletonBase<T> where T : class
	{
		#region Methods

		#region Init

		protected abstract void Init();

		#endregion

		#endregion
	}
}
