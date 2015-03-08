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

namespace DatabaseFramework.Database.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataDefinitionAttribute : Attribute
	{
		#region Fields

		private string _databaseName;
		private string _tableName;

		#endregion

		#region Properties

		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}

		public string DatabaseName
		{
			get { return _databaseName; }
			set { _databaseName = value; }
		}

		#endregion

		#region Constructors

		public DataDefinitionAttribute(string databaseName, string tableName)
		{
			DatabaseName = databaseName;
			TableName = tableName;
		}

		#endregion
	}
}
