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

using DatabaseFramework.Database.Base;
using DatabaseFramework.Database.Misc;
using MySql.Data.MySqlClient;
using System;
using System.Linq;

namespace DatabaseFramework.Managers.Core
{
	public static class DatabaseManager
	{
		#region Methods

		#region GetCollection<T>

		public static ResultBase<T> GetCollection<T>(IDatabaseProvider provider, string procedureName, params object[] args)
		{
			ResultBase<T> retVal = new ResultBase<T>();

			using (provider)
			{
				using (MySqlTransaction transaction = provider.Connection.BeginTransaction())
				{
					try
					{
						MySqlCommand command = new MySqlCommand
							(
								String.Format
									(
										"CALL {0}({1})"
									,	procedureName
									,	String.Join(", ", args.Select(x => Convert.ToString(x)))
									)
							,	provider.Connection
							,	transaction
							);

						using (MySqlDataReader reader = command.ExecuteReader())
						{
							retVal.LoadData(reader);
						}
					}
					catch (MySqlException)
					{
						transaction.Rollback();
					}

					transaction.Commit();
				}
			}

			return retVal;
		}

		#endregion

		#region Get<T>

		public static T Get<T>(IDatabaseProvider provider, string procedureName, params object[] args)
		{
			T retVal = default(T);

			retVal = GetCollection<T>(provider, procedureName, args).FirstOrDefault();

			return retVal;
		}

		#endregion

		#endregion	
	}
}
