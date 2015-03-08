using System;

namespace DatabaseFramework.Database.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TableAttribute : Attribute
	{
		#region Fields

		private string _databaseName;
		private string _tableName;

		#endregion

		#region Properties

		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}

		public string DatabaseName
		{
			get { return _databaseName; }
			set { _databaseName = value; }
		}

		#endregion

		#region Constructors

		public TableAttribute(string databaseName, string tableName)
		{
			DatabaseName = databaseName;
			TableName = tableName;
		}

		#endregion
	}
}
