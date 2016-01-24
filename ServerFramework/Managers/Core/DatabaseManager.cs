/*
 * Copyright (c) 2016. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Context;
using ServerFramework.Database.Base.Entity;
using ServerFramework.Managers.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ServerFramework.Managers.Core
{
	public class DatabaseManager : ManagerBase<DatabaseManager>
	{
		#region Constructors

		DatabaseManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		protected override void Init()
		{
		}

		#endregion

		#region AddOrUpdate

		public void AddOrUpdate<T>(DBContextBase context, bool saveChanges, params T[] entities)
			where T : class, IEntity
		{
			context.Add(entities);

			if (saveChanges)
				context.SaveChanges();
		}

		#endregion

		#region Remove

		public void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			context.Remove(func);

			if (saveChanges)
				context.SaveChanges();
		}

		public void Remove<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			context.Remove(func);

			if (saveChanges)
				context.SaveChanges();
		}

		#endregion

		#region Get

		public T Get<T>(DBContextBase context, Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			return context.Get(func);
		}

		public IEnumerable<T> Get<T>(DBContextBase context, Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			return context.Get(func);
		}

		#endregion

		#region Update

		public void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, T> func, Action<T> action)
			where T : class, IEntity
		{
			context.Update(func, action);

			if (saveChanges)
				context.SaveChanges();
		}

		public void Update<T>(DBContextBase context, bool saveChanges, Func<DbSet<T>, IEnumerable<T>> func, Action<T> action)
			where T : class, IEntity
		{
			context.Update(func, action);

			if (saveChanges)
				context.SaveChanges();
		}

		#endregion

		#endregion
	}
}
