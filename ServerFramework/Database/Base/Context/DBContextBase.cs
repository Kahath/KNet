/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using ServerFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerFramework.Database.Base.Context
{
	public abstract class DBContextBase : DbContext
	{
		#region Properties

		public string ConnectionString
		{
			get { return Database.Connection.ConnectionString; }
		}

		public abstract Dictionary<Type, DbSet> DbSetMap { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instnce of <see cref="ServerFramework.Database.Base.DBContextBase"/> type.
		/// </summary>
		/// <param name="connectionString">Database connection string.</param>
		protected DBContextBase(string connectionString)
			: base(connectionString)
		{
		}

		#endregion

		#region Methods

		#region Add

		private void Add<T>(T entity) where T : class, IEntity
		{
			if (entity != null)
			{
				if (entity.DateCreated != null && entity.DateCreated != DateTime.MinValue)
				{
					DbEntityEntry<T> entry = Entry(entity);
					entry.State = EntityState.Modified;
				}
				else
				{
					DbSet<T> set = GetEntitySet<T>(entity.GetType());
					set.Add(entity);
				}
			}
		}

		internal void Add<T>(IEnumerable<T> entities) where T : class, IEntity
		{
			if (entities != null && entities.Any())
			{
				DbSet<T> set = GetEntitySet<T>(entities.First().GetType());
				IEnumerable<T> addEntities = entities.Where(x => x.DateCreated == null || x.DateCreated == DateTime.MinValue);
				IEnumerable<T> updateEntities = entities.Where(x => x.DateCreated != null && x.DateCreated != DateTime.MinValue);

				if(addEntities != null && addEntities.Any())
					set.AddRange(addEntities);

				if (updateEntities != null && updateEntities.Any())
				{
					foreach (T entity in updateEntities)
						Add(entity);
				}
			}
		}

		#endregion

		#region Remove

		internal void Remove<T>(Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(typeof(T));
			IEnumerable<T> entities = func(set);

			if (entities != null && entities.Any())
				set.RemoveRange(entities);
		}

		internal void Remove<T>(Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(typeof(T));
			T entity = func(set);

			if (entity != null)
				set.Remove(entity);
		}

		#endregion

		#region Get

		internal T Get<T>(Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			T retVal = default(T);

			DbSet<T> set = GetEntitySet<T>(typeof(T));
			retVal = func(set);

			return retVal;
		}

		internal IEnumerable<T> Get<T>(Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			IEnumerable<T> retVal = Enumerable.Empty<T>();

			DbSet<T> set = GetEntitySet<T>(typeof(T));
			retVal = func(set);

			return retVal;
		}

		#endregion

		#region Update

		internal void Update<T>(Func<DbSet<T>, T> func, Action<T> action)
			where T : class, IEntity
		{
			T entity = Get(func);

			if(entity != null)
			{
				action(entity);
				Add(entity);
			}
		}

		internal void Update<T>(Func<DbSet<T>, IEnumerable<T>> func, Action<T> action)
			where T : class, IEntity
		{
			IEnumerable<T> entities = Get(func);

			if(entities != null && entities.Any())
			{
				foreach (T entity in entities)
					action(entity);

				Add(entities);
			}
		}

		#endregion

		#region ValidateEntitySet

		private void ValidateEntitySet(Type type, DbSet set)
		{
			if (set == null)
				throw new DatabaseException($"Given entity Set '{type}' is not present in DbSetMap");
		}

		#endregion

		#region GetEntitySet

		private DbSet<T> GetEntitySet<T>(T entity)
			where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(entity.GetType());

			return set;
		}

		private DbSet<T> GetEntitySet<T>(Type type)
			where T : class, IEntity
		{
			DbSet set = null;
			Type types = typeof(T);
			DbSetMap.TryGetValue(type, out set);

			ValidateEntitySet(type, set);

			return set.Cast<T>();
		}

		#endregion

		#region SaveChanges

		/// <summary>
		/// Adds DateCreated to added entites, DateModified to modified entities and deactivates deleted entities.
		/// Saves all changes from context to underlying database.
		/// </summary>
		/// <returns>
		/// The number of state entries written to the underlying database. This can
		/// include state entries for entities and/or relationships. Relationship state
		/// entries are created for many-to-many relationships and relationships where
		/// there is no foreign key property included in the entity class (often referred
		/// to as independent associations)
		/// </returns>
		public override int SaveChanges()
		{
			PreSave();

			return base.SaveChanges();
		}

		#endregion

		#region SaveChangesAsync

		public override Task<int> SaveChangesAsync()
		{
			PreSave();

			return base.SaveChangesAsync();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			PreSave();

			return base.SaveChangesAsync(cancellationToken);
		}

		#endregion

		#region PreSave

		private void PreSave()
		{
			ObjectContext context = ((IObjectContextAdapter)this).ObjectContext;

			IEnumerable<ObjectStateEntry> objectStateEntries =
				context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted).Where
				(
					x => !x.IsRelationship
					&& x.Entity != null
					&& typeof(IEntity).IsAssignableFrom(x.Entity.GetType())
				);

			DateTime date = DateTime.Now;

			foreach (ObjectStateEntry entry in objectStateEntries)
			{
				IEntity entity = entry.Entity as IEntity;

				if (entity != null)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							entity.DateCreated = date;
							entity.Active = true;
							break;
						case EntityState.Modified:
							entity.DateModified = date;
							break;
						case EntityState.Deleted:
							entry.ChangeState(EntityState.Modified);
							entity.DateDeactivated = date;
							entity.Active = false;
							break;
					}
				}
			}
		}

		#endregion

		#endregion
	}
}
