using System.Reflection;

namespace DatabaseFramework.Database.Core
{
	public sealed class Property
	{
		#region Fields

		private string _columnName;
		private FieldInfo _value;

		#endregion

		#region Properties

		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		public FieldInfo Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Constructors

		public Property(string name, FieldInfo value)
		{
			ColumnName = name;
			Value = value;
		}

		#endregion
	}
}
