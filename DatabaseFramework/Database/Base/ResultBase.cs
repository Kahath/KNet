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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DatabaseFramework.Database.Base
{
	public class ResultBase<T> : List<T>
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

		public ResultBase()
		{
			_constructor = typeof(T).GetConstructor(Type.EmptyTypes);
			_properties = GetProperties();
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
							property.Value.SetValue(result, reader[property.ColumnName]);
						}
						catch (IndexOutOfRangeException)
						{
							property.Value.SetValue(result, null);
						}
					}

					this.Add(result);
				}
			}
		}

		#endregion

		#region GetProperties

		private static IEnumerable<Property> GetProperties()
		{
			List<Property> retVal = new List<Property>();

			IEnumerable<FieldInfo> fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			fields = fields.Concat(typeof(T).BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));
			IEnumerable<ColumnAttribute> columnAttributes = fields.Select(x => x.GetCustomAttribute<ColumnAttribute>());

			for (int i = 0; i < fields.Count(); i++)
			{
				Property property = new Property
					(
						columnAttributes.ElementAt(i).Name
					,	fields.ElementAt(i)
					);

				yield return property;
			}
		}

		#endregion

		#endregion
	}
}
