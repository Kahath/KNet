/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ServerFramework.Database.Base
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
		///</returns>
		public override int SaveChanges()
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

			return base.SaveChanges();
		}

		#endregion

		#endregion
	}
}
