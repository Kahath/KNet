﻿/*
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

using System.Reflection;

namespace DatabaseFramework.Database.Core
{
	public sealed class Property
	{
		#region Fields

		private string _columnName;
		private FieldInfo _value;

		#endregion

		#region Properties

		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		public FieldInfo Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Constructors

		public Property(string name, FieldInfo value)
		{
			ColumnName = name;
			Value = value;
		}

		#endregion
	}
}
