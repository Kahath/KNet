using DatabaseFramework.Database.Attributes;
using DatabaseFramework.Database.Base;
using DatabaseFramework.Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DatabaseFramework.Database.Helpers
{
	public static class DALHelpers
	{
		#region Methods

		#region GetProperties<T>

		public static IEnumerable<Property> GetProperties<T>(object item = null)
		{
			Type type = typeof(T);

			if(item != null)
				type = item.GetType();

			List<Property> retVal = new List<Property>();

			IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			fields = fields.Concat(type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));
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

		#region CreateSaveQuery<T>

		public static string CreateSaveQuery<T>(DataObjectBase item)
		{
			StringBuilder retVal = new StringBuilder();

			if (DataObjectBase.Properties == null)
			{
				IEnumerable<Property> properties = GetProperties<object>(item);
				DataObjectBase.Properties = properties;
			}

			string insertValues = String.Empty;
			string updateValues = String.Empty;

			insertValues = String.Join(", ", DataObjectBase.Properties.Select(x =>
						x.FieldInfo.GetValue(item) != null ?
						x.FieldInfo.GetValue(item) is string
					|| x.FieldInfo.GetValue(item) is DateTime ?
					"\'" + x.FieldInfo.GetValue(item).ToString() + "\'"
					: x.FieldInfo.GetValue(item).ToString()
					: "NULL"));

			updateValues = String.Join(", ", DataObjectBase.Properties.Select(x =>
						x.FieldInfo.GetValue(item) != null ?
						x.FieldInfo.GetValue(item) is string ?
						'`' + x.ColumnName + "`= \'" + x.FieldInfo.GetValue(item).ToString() + "\'"
					:	x.FieldInfo.GetValue(item) is DateTime ?
						'`' + x.ColumnName +"`= \'" + ((DateTime)x.FieldInfo.GetValue(item)).ToString("yyyy-MM-dd HH:mm:ss") + "\'"
					: '`' + x.ColumnName + "`= " + x.FieldInfo.GetValue(item)
					: '`' + x.ColumnName + "`= NULL"));

			DataDefinitionAttribute dataDefinitionAttribute
				= Attribute.GetCustomAttribute(item.GetType(), typeof(DataDefinitionAttribute))
				as DataDefinitionAttribute;

			retVal.AppendFormat("INSERT INTO {0} ({1}) VALUES({2}) ON DUPLICATE KEY UPDATE {3}"
				, '`' + dataDefinitionAttribute.TableName + '`'
				, String.Join(",", DataObjectBase.Properties.Select(x => '`' + x.ColumnName + '`'))
				, insertValues
				, updateValues);

			return retVal.ToString();
		}

		#endregion

		#endregion

	}
}
