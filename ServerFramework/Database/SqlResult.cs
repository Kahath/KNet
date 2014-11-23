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

        public T Read<T>(int row, string column)
        {
            return (T)Convert.ChangeType(Rows[row][column], typeof(T));
        }

        public object[] ReadAllValuesFromField(string column)
        {
            object[] o = new object[Count];

            for (int i = 0; i < Count; i++)
                o[i] = Rows[i][column];

            return o;
        }

        #endregion     
    }
}
