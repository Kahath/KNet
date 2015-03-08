using MySql.Data.MySqlClient;
using System;

namespace DatabaseFramework.Database.Misc
{
	public interface IDatabaseProvider : IDisposable
	{
		string ConnectionString
		{
			get;
			set;
		}

		MySqlConnection Connection
		{
			get;
			set;
		}
	}
}
