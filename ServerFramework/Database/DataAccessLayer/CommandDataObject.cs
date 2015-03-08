using DatabaseFramework.Database.Attributes;
using DatabaseFramework.Database.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Database.DataAccessLayer
{
	[Table("kahath.application", "command")]
	public sealed class CommandDataObject : DataObjectBase
	{
		#region Fields

		[Column("Name")]
		private string _name;

		[Column("CommandLevel")]
		private ushort? _commandLevel;

		[Column("Description")]
		private string _description;

		#endregion

		#region Properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public ushort? CommandLevel
		{
			get { return _commandLevel; }
			set { _commandLevel = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		#endregion
	}
}
