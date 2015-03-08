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
using System;

namespace DatabaseFramework.Database.Base
{
	public abstract class DataObjectBase
	{
		#region Fields

		[Column("ID")]
		private uint? _id;

		[Column("Active")]
		private bool? _active;

		[Column("DateCreated")]
		private DateTime? _dateCreated;

		#endregion

		#region Properties

		protected uint? ID
		{
			get { return _id; }
			set { _id = value; }
		}

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

		#endregion

		#region Constructors

		public DataObjectBase()
		{

		}

		#endregion

	}
}
