/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
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
			UpdateBeforeSave();

			return base.SaveChanges();
		}

		#endregion

		#region SaveChangesAsync

		public override Task<int> SaveChangesAsync()
		{
			UpdateBeforeSave();

			return base.SaveChangesAsync();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			UpdateBeforeSave();

			return base.SaveChangesAsync(cancellationToken);
		}

		#endregion

		#region UpdateBeforeSave

		private void UpdateBeforeSave()
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
