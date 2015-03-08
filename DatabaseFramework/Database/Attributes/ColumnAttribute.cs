using System;

namespace DatabaseFramework.Database.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ColumnAttribute : Attribute
	{
		#region Fields

		private string _name;

		#endregion

		#region Properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		#region Constructors

		public ColumnAttribute(string name)
		{
			Name = name;
		}

		#endregion
	}
}
