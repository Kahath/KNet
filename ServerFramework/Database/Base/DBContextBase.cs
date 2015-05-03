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
using System.Data.Entity;
using System.IO;
using System.Reflection;

namespace ServerFramework.Database.Base
{
    public class DBContextBase : DbContext
    {
        #region Fields

        private string _connectionString;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion

        #region Constructors

        public DBContextBase(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;
            Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion

        #region Methods

        public async void Save()
        {
            await SaveChangesAsync();
        }

        #endregion
    }
}
