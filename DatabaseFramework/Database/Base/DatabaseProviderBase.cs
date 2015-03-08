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

using DatabaseFramework.Database.Misc;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseFramework.Database.Base
{
	public abstract class DatabaseProviderBase : IDatabaseProvider
	{
		#region Fields

		private MySqlConnection _connection;
		private string _connectionString;

		#endregion

		#region Properties

		public string ConnectionString
		{
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		public MySqlConnection Connection
		{
			get { return _connection; }
			set { _connection = value; }
		}

		#endregion

		#region Constructors

		public DatabaseProviderBase()
		{

		}

		#endregion

		#region Methods

		public void Init(string host, string user,
			string pass, int port, string database)
		{
			ConnectionString = String.Format("server={0};port={1};database={2};uid={3};pwd={4}"
				, host, port, database, user, pass);

			Connection = new MySqlConnection(ConnectionString);
			Connection.Open();
		}

		public void Dispose()
		{
			//Connection.Close();
		}

		#endregion


	}
}
