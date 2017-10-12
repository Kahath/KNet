/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using DILibrary.DependencyInjection;
using DILibrary.Constants;
using KNetFramework.Managers.Interface;

namespace KNetFramework.Managers.Base
{
	public abstract class ManagerBase<T, K> : Dependency<T, K>
		where T : Dependency<T, K>, new()
		where K : class, IManager
	{
		#region Constructors

		public ManagerBase()
			: base(ResolveType.Singleton)
		{

		}

		#endregion

		#region Methods

		#region Init

		protected void Init()
		{
			Instance.Init();
		}

		#endregion

		#endregion
	}
}
