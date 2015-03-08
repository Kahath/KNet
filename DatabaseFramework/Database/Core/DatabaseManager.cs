using DatabaseFramework.Database.Attributes;
using DatabaseFramework.Database.Base;
using DatabaseFramework.Database.Core;
using DatabaseFramework.Database.Misc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Managers.Core
{
	public static class DatabaseManager
	{
		public static ResultBase<T> GetCollection<T>(IDatabaseProvider provider, string procedureName, params object[] args)
		{
			ResultBase<T> retVal = new ResultBase<T>();
			Type tableType = typeof(T);
			T result = default(T);

			IEnumerable<Property> properties = GetProperties(tableType);
			ConstructorInfo constructorInfo = tableType.GetConstructor(Type.EmptyTypes);
			TableAttribute tableAttribute = tableType.GetCustomAttribute<TableAttribute>();

			using (provider)
			{
				using (MySqlTransaction transaction = provider.Connection.BeginTransaction())
				{
					try
					{
						MySqlCommand command = new MySqlCommand(String.Format("Call {0}({1})"
							, procedureName
							, String.Join(", ", args.Select(x => Convert.ToString(x))))
							, provider.Connection
							, transaction);

						using (MySqlDataReader reader = command.ExecuteReader())
						{
							if (reader.HasRows)
							{
								while (reader.Read())
								{
									object invokedConstructor = constructorInfo.Invoke(null);
									result = (T)Convert.ChangeType(invokedConstructor, tableType);

									foreach (Property property in properties)
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

									retVal.Add(result);
								}
							}
						}
					}
					catch(MySqlException)
					{
						transaction.Rollback();
					}

					transaction.Commit();
				}
			}

			return retVal;
		}

		public static T Get<T>(IDatabaseProvider provider, string procedureName, params object[] args)
		{
			T retVal = default(T);

			retVal = GetCollection<T>(provider, procedureName, args).FirstOrDefault();

			return retVal;
		}

		private static IEnumerable<Property> GetProperties(Type tableType)
		{
			List<Property> retVal = new List<Property>();

			IEnumerable<FieldInfo> fields = tableType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			fields = fields.Concat(tableType.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));
			IEnumerable<ColumnAttribute> columnAttributes = fields.Select(x => x.GetCustomAttribute<ColumnAttribute>());

			for (int i = 0; i < fields.Count(); i++)
			{
				Property property = new Property
					(
						columnAttributes.ElementAt(i).Name
					,	fields.ElementAt(i)
					);

				//retVal.Add(property);
				yield return property;
			}

			//return retVal;
		}
	}
}
