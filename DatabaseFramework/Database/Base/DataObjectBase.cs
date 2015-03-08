
using DatabaseFramework.Database.Attributes;
using System;

namespace DatabaseFramework.Database.Base
{
	public abstract class DataObjectBase
	{
		#region Fields

		[Column("ID")]
		private uint? _id;

		[Column("Active")]
		private bool? _active;

		[Column("DateCreated")]
		private DateTime? _dateCreated;

		#endregion

		#region Properties

		protected uint? ID
		{
			get { return _id; }
			set { _id = value; }
		}

		protected bool? Active
		{
			get { return _active; }
			set { _active = value; }
		}

		protected DateTime? DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}

		#endregion

		#region Constructors

		public DataObjectBase()
		{

		}

		#endregion

	}
}
