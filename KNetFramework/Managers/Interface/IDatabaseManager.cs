/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Context;
using KNetFramework.Database.Base.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace KNetFramework.Managers.Interface
{
	public interface IDatabaseManager : IManager
	{
		#region Methods

		#region AddOrUpdate

		void AddOrUpdate<T, K>(params K[] entities)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void AddOrUpdate<T>(DBContextBase context, bool saveChanges, params T[] entities)
			where T : class, IEntity;

		#endregion

		#region Remove

		void Remove<T, K>(Func<DbSet<K>, K> func)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Remove<T, K>(Func<DbSet<K>, IEnumerable<K>> func)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func)
			where T : class, IEntity;
		void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity;
		void Remove<T, K>(K entity)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Remove<T, K>(IEnumerable<K> entities)
			where T : DBContextBase, new()
			where K : class, IEntity;

		#endregion

		#region Get

		K Get<T, K>(Func<DbSet<K>, K> func)
			where T : DBContextBase, new()
			where K : class, IEntity;
		IEnumerable<K> Get<T, K>(Func<DbSet<K>, IEnumerable<K>> func)
			where T : DBContextBase, new()
			where K : class, IEntity;
		T Get<T>(DBContextBase context, Func<DbSet<T>, T> func)
			where T : class, IEntity;
		IEnumerable<T> Get<T>(DBContextBase context, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity;

		#endregion

		#region Update

		void Update<T, K>(K entity)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Update<T, K>(IEnumerable<K> entities)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Update<T, K>(Func<DbSet<K>, K> func, Action<K> action)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Update<T, K>(Func<DbSet<K>, IEnumerable<K>> func, Action<K> action)
			where T : DBContextBase, new()
			where K : class, IEntity;
		void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func, Action<T> action)
			where T : class, IEntity;
		void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func, Action<T> action)
			where T : class, IEntity;

		#endregion

		#endregion
	}
}
