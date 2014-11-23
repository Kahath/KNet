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

        internal MySqlBase()
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
                Log.Message(LogType.Normal, "Successfuly tested database connection on {0}:{1} {2}",
                    host, port, database);
                pingTimer.Elapsed += pingTimer_Elapsed;
                pingTimer.Start();
            }
            catch (MySqlException e)
            {
                Log.Message(LogType.Error, e.Message);
                Log.Message(LogType.Database, "Trying to reconnect in 5s...");
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
                    var mParams = new List<MySqlParameter>(args.Length);

                    foreach (var a in args)
                        mParams.Add(new MySqlParameter("", a));

                    command.Parameters.AddRange(mParams.ToArray());
                    command.ExecuteNonQuery();
                    Log.Message(LogType.Database, "Successfuly executed: \"{0}\"", sqlString);
                    return true;
                }
            }
            catch (MySqlException e)
            {
                Log.Message(LogType.Error, e.Message);
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
                            Log.Message(LogType.Database, "Successfuly executed: \"{0}\"", sqlString);
                            return retData;
                        }
                    }
                }

            }
            catch (MySqlException e)
            {
                Log.Message(LogType.Error, e.Message);
                return null;
            }
        }

        #endregion

        #region MyRegion

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
                Log.Message(LogType.Database, "Successfuly executed: \"{0}\"", sqlString.ToString());
            }
            catch (MySqlException e)
            {
                Log.Message(LogType.Error, e.Message);
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
            Log.Message(LogType.Normal, "Pinging database");
            connection.Ping();
        }

        #endregion

        #endregion
    }
}
