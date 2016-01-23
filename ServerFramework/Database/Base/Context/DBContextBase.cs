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

		internal void Add<T>(T entity) where T : class, IEntity
		{
			if (entity.DateCreated != null && entity.DateCreated != DateTime.MinValue)
			{
				DbEntityEntry<T> entry = Entry(entity);
				entry.State = EntityState.Modified;
			}
			else
			{
				Type types = typeof(T);
				DbSet<T> set = GetEntitySet<T>(entity.GetType());
				set.Add(entity);
			}
		}

		internal void Add<T>(T[] entities)
			where T : class, IEntity
		{
			if (entities.Any())
			{
				if (entities.Count() > 1)
				{
					AddRange(entities);
				}
				else
				{
					Add(entities.First());
				}
			}
		}

		private void AddRange<T>(IEnumerable<T> entities) where T : class, IEntity
		{
			ValidateEntities(entities);

			DbSet<T> set = GetEntitySet<T>(entities.First().GetType());
			set.AddRange(entities);
		}

		#endregion

		#region Remove

		private void Remove<T>(T entity) where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(entity.GetType());
			set.Remove(entity);
		}

		internal void Remove<T>(Func<DbSet<T>, IEnumerable<T>> func)
			where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(typeof(T));
			RemoveRange(func(set));
		}

		internal void Remove<T>(Func<DbSet<T>, T> func)
			where T : class, IEntity
		{
			DbSet<T> set = GetEntitySet<T>(typeof(T));
			Remove(func(set));
		}

		private void RemoveRange<T>(IEnumerable<T> entities) where T : class, IEntity
		{
			if (entities.Any())
			{
				ValidateEntities(entities);

				DbSet<T> set = GetEntitySet<T>(entities.First().GetType());
				set.RemoveRange(entities);
			}
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

		internal IEnumerable<T> Get<T>(Func<DbSet<T>, IEnumerable<T>> func, bool asNoTracking = false)
			where T : class, IEntity
		{
			IEnumerable<T> retVal = Enumerable.Empty<T>();

			DbSet<T> set = GetEntitySet<T>(typeof(T));
			retVal = func(set);

			return retVal;
		}

		#endregion

		#region ValidateEntity

		private void ValidateEntities<T>(IEnumerable<T> entities)
		{
			bool isValid = entities.Select(x => x.GetType()).Distinct().Skip(1).Any();

			if (!isValid)
				throw new DatabaseException("Entities are not valid for range remove");
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
