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
