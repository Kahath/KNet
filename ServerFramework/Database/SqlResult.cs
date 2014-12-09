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
using System.Data;

namespace ServerFramework.Database
{
    public class SqlResult : DataTable
    {
        #region Fields

        public int _count;

        #endregion

        #region Properties

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        #endregion

        #region Methods

        #region Read

        public T Read<T>(int row, string column)
        {
            return (T)Convert.ChangeType(Rows[row][column], typeof(T));
        }

        #endregion

        #region ReadAllValuesFromField

        public object[] ReadAllValuesFromField(string column)
        {
            object[] o = new object[Count];

            for (int i = 0; i < Count; i++)
                o[i] = Rows[i][column];

            return o;
        }

        #endregion

        #endregion     
    }
}
