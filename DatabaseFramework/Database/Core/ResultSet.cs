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

using DatabaseFramework.Database.Helpers;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DatabaseFramework.Database.Core
{
	public sealed class ResultSet<T> : List<T>
	{
		#region Fields

		private readonly ConstructorInfo _constructor;
		private readonly IEnumerable<Property> _properties;

		#endregion

		#region Properties

		internal ConstructorInfo Constructor
		{
			get { return _constructor; }
		}

		internal IEnumerable<Property> Properties
		{
			get { return _properties; }
		}

		#endregion

		#region Constructors

		public ResultSet()
		{
			_constructor = typeof(T).GetConstructor(Type.EmptyTypes);
			_properties =  DALHelpers.GetProperties<T>();
		}

		#endregion

		#region Methods

		#region LoadData

		internal void LoadData(MySqlDataReader reader)
		{
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					object invokedConstructor = Constructor.Invoke(null);
					T result = (T)Convert.ChangeType(invokedConstructor, typeof(T));

					foreach (Property property in Properties)
					{
						try
						{
							property.FieldInfo.SetValue(result, reader[property.ColumnName]);
						}
						catch (Exception e)
						{
							if (
									e is MySqlConversionException 
								||	e is IndexOutOfRangeException
								||	e is ArgumentException)
								property.FieldInfo.SetValue(result, null);
							else
								throw;
						}
					}

					Add(result);
				}
			}
		}

		#endregion

		#endregion
	}
}
