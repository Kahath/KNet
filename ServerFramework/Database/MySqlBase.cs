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

using MySql.Data.MySqlClient;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace ServerFramework.Database
{
    public class MySqlBase : IDisposable
    {
        #region Fields

        string connectionString;
        MySqlConnection connection;
        System.Timers.Timer pingTimer;

        #endregion

        #region Constructors

        public MySqlBase()
        {
            pingTimer = new System.Timers.Timer(300000);
            pingTimer.AutoReset = true;
        }

        #endregion

        #region Methods

        #region Init

        public void Init(string host, string user,
            string pass, int port, string database)
        {
            connectionString = "Server=" + host + ";Port=" + port +
                ";Database=" + database + ";Uid=" + user + ";Pwd=" + pass;

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                LogManager.Log(LogType.Normal, "Successfuly tested database connection on {0}:{1} {2}",
                    host, port, database);
                pingTimer.Elapsed += pingTimer_Elapsed;
                pingTimer.Start();
            }
            catch (MySqlException e)
            {
                LogManager.Log(LogType.Error, e.Message);
                LogManager.Log(LogType.Database, "Trying to reconnect in 5s...");
                Thread.Sleep(5000);

                Init(host, user, pass, port, database);
            }
        }

        #endregion

        #region Execute

        public bool Execute(string sqlString, params object[] args)
        {
            try
            {
                using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                {
                    if (args != null && args.Length > 0)
                    {
                        var mParams = new List<MySqlParameter>(args.Length);

                        foreach (var a in args)
                            mParams.Add(new MySqlParameter("", a));

                        command.Parameters.AddRange(mParams.ToArray());
                    }

                    command.ExecuteNonQuery();
                    LogManager.Log(LogType.Database, "Successfuly executed: \"{0}\"", sqlString);
                    return true;
                }
            }
            catch (MySqlException e)
            {
                LogManager.Log(LogType.Error, e.Message);
                return false;
            }
        }

        #endregion

        #region Select

        public SqlResult Select(string sqlString, params object[] args)
        {
            try
            {
                using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                {
                    var mParams = new List<MySqlParameter>(args.Length);

                    foreach (var a in args)
                        mParams.Add(new MySqlParameter("", a));

                    command.Parameters.AddRange(mParams.ToArray());

                    using (var sqlData = command.ExecuteReader())
                    {
                        using (SqlResult retData = new SqlResult())
                        {
                            retData.Load(sqlData);
                            retData.Count = retData.Rows.Count;
                            LogManager.Log(LogType.Database, "Successfuly executed: \"{0}\"", sqlString);
                            return retData;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                LogManager.Log(LogType.Error, e.Message);
                return null;
            }
        }

        #endregion

        #region ExecuteBigQuery

        public void ExecuteBigQuery(string table, string fields,
           int resultCount, object[] values)
        {
            if (values.Length < 0)
                return;

            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendFormat("INSERT INTO {0} ({1}) VALUES", table, fields);

            for (int i = 0; i < resultCount; i++)
            {
                sqlString.AppendFormat("(");
                sqlString.AppendFormat("{0}", values[i]);

                if (i == resultCount - 1)
                    sqlString.AppendFormat(");");
                else
                    sqlString.AppendFormat("),");
            }

            try
            {
                MySqlCommand command = new MySqlCommand(sqlString.ToString(), connection);
                command.ExecuteNonQuery();
                LogManager.Log(LogType.Database, "Successfuly executed: \"{0}\"", sqlString.ToString());
            }
            catch (MySqlException e)
            {
                LogManager.Log(LogType.Error, e.Message);
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            connection.Close();
            connectionString = null;
        }

        #endregion

        #endregion

        #region +EventHandling

        #region pingTimer_Elapsed

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LogManager.Log(LogType.Normal, "Pinging database");
            connection.Ping();
        }

        #endregion

        #endregion
    }
}
