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

using ServerFramework.Configuration;
using ServerFramework.Database.Context;

namespace ServerFramework.Database
{
    public static class DB
    {
        private static ApplicationContext _applicationContext;

        public static ApplicationContext ApplicationContext
        {
            get
            {
                ApplicationContext retVal = null;

                if (_applicationContext == null)
                {
                    retVal = _applicationContext = new ApplicationContext(ServerConfig.GetConnectionString());
                }
                else
                {
                    retVal = _applicationContext;
                }

                return retVal;
            }
        }
    }
}
