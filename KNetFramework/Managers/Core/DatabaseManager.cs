/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Context;
using KNetFramework.Database.Base.Entity;
using KNetFramework.Managers.Base;
using KNetFramework.Managers.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace KNetFramework.Managers.Core
{
	public class DatabaseManager : ManagerBase<DatabaseManager, IDatabaseManager>
	{
		#region Methods

		#region AddOrUpdate

		public void AddOrUpdate<T, K>(params K[] entities)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.AddOrUpdate<T, K>(entities);
		}

		public void AddOrUpdate<T>(DBContextBase context, bool saveChanges, params T[] entities)
			where T : class, IEntity
		{
			Instance.AddOrUpdate(context, saveChanges, entities);
		}

		#endregion

		#region Remove

		public void Remove<T, K>(Func<DbSet<K>, K> func)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Remove<T, K>(func);
		}

		public void Remove<T, K>(Func<DbSet<K>, IEnumerable<K>> func)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Remove<T, K>(func);
		}

		public void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			Instance.Remove(context, saveChanges, func);
		}

		public void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			Instance.Remove(context, saveChanges, func);
		}

		public void Remove<T, K>(K entity)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Remove<T, K>(entity);
		}

		public void Remove<T, K>(IEnumerable<K> entities)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Remove<T, K>(entities);
		}

		#endregion

		#region Get

		public K Get<T, K>(Func<DbSet<K>, K> func)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			return Instance.Get<T, K>(func);
		}

		public IEnumerable<K> Get<T, K>(Func<DbSet<K>, IEnumerable<K>> func)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			return Instance.Get<T, K>(func);
		}

		public T Get<T>(DBContextBase context, Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			return Instance.Get(context, func);
		}

		public IEnumerable<T> Get<T>(DBContextBase context, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			return Instance.Get<T>(context, func);
		}

		#endregion

		#region Update

		public void Update<T, K>(K entity)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Update<T, K>(entity);
		}

		public void Update<T, K>(IEnumerable<K> entities)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Update<T, K>(entities);
		}

		public void Update<T, K>(Func<DbSet<K>, K> func, Action<K> action)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Update<T, K>(func, action);
		}

		public void Update<T, K>(Func<DbSet<K>, IEnumerable<K>> func, Action<K> action)
			where T : DBContextBase, new()
			where K : class, IEntity
		{
			Instance.Update<T, K>(func, action);
		}

		public void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func, Action<T> action)
			where T : class, IEntity
		{
			Instance.Update<T>(context, saveChanges, func, action);
		}

		public void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func, Action<T> action)
			where T : class, IEntity
		{
			Instance.Update<T>(context, saveChanges, func, action);
		}

		#endregion

		#endregion
	}
}
