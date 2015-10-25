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

using ServerFramework.Database.Base.Context;
using ServerFramework.Database.Base.Entity;
using System;

namespace ServerFramework.Database.Base.Repository
{
	public abstract class RepositoryBase<T> : IDisposable where T : IEntity
	{
		#region Fields

		private DBContextBase _context;

		#endregion

		#region Properties

		protected DBContextBase Context
		{
			get { return _context; }
			set { _context = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="ServerFramework.Database.Base.RepositoryBase{T}"/> type.
		/// </summary>
		/// <param name="context">Instance of <see cref="ServerFramework.Database.Base.DBContextBase"/> type.</param>
		public RepositoryBase(DBContextBase context)
		{
			Context = context;
		}

		#endregion

		#region Methods

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes object
		/// </summary>
		/// <param name="disposing">disposing</param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				Context.Dispose();
			}
		}

		#endregion

		#endregion
	}
}
