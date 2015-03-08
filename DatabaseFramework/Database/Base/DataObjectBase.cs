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

using DatabaseFramework.Database.Attributes;
using DatabaseFramework.Database.Core;
using DatabaseFramework.Database.Misc;
using DatabaseFramework.Managers.Core;
using System;
using System.Collections.Generic;

namespace DatabaseFramework.Database.Base
{
	public abstract class DataObjectBase
	{
		#region Fields

		[Column("Active")]
		private bool? _active;

		[Column("DateCreated")]
		private DateTime? _dateCreated;

		private static IEnumerable<Property> _properties; 

		#endregion

		#region Properties

		protected bool? Active
		{
			get { return _active; }
			set { _active = value; }
		}

		protected DateTime? DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}

		internal static IEnumerable<Property> Properties
		{
			get { return _properties; }
			set { _properties = value; }
		}

		#endregion

		#region Methods

		public void Save(IDatabaseProvider provider)
		{
			DatabaseManager.Save<DataObjectBase>(provider, this);
		}

		#endregion
	}
}
