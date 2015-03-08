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
using System.Data;

namespace DatabaseFramework.Database.Base
{
	public abstract class ConnectionProviderBase : IDatabaseProvider
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
		}

		#endregion

		#region Methods

		#region Init

		protected void Init()
		{
			_connection = new MySqlConnection(ConnectionString);
		}

		#endregion

		#region Open

		protected void Open()
		{
			if(Connection != null && Connection.State != ConnectionState.Open)
				Connection.Open();
		}

		#endregion

		#region Close

		protected void Close()
		{
			if (Connection != null && Connection.State != ConnectionState.Closed)
				Connection.Close();
		}

		#endregion

		#region Dispose

		public void Dispose()
		{
			//Connection.Close();
		}

		#endregion

		#endregion
	}
}
