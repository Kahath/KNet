/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Log
{
	[Table("Log", Schema = "KNet")]
	public class LogModel : EntityBase<int>
	{
		#region Properties

		public string Message		{ get; set; }
		public string Description	{ get; set; }
		public int LogTypeID		{ get; set; }

		[ForeignKey("LogTypeID")]
		public LogTypeModel LogType { get; set; }

		#endregion
	}
}
