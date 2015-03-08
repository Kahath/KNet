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
using System;

namespace ServerFramework.Database.Providers
{
	public class ApplicationProvider : ConnectionProviderBase
	{
		#region Methods

		#region Init

		public void Init(string host, string user, string pass, int port, string database)
		{
			ConnectionString = String.Format("server={0};port={1};database={2};uid={3};pwd={4};Convert Zero Datetime=True"
				, host, port, database, user, pass);

			Init();
			OpenConnection();
		}

		#endregion

		#region OpenConnection

		public void OpenConnection()
		{
			Open();
		}

		#endregion

		#region CloseConnection

		public void CloseConnection()
		{
			Close();
		}

		#endregion

		#endregion
	}
}
