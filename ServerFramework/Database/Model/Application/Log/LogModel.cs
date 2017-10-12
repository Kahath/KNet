/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Log
{
	[Table("Log", Schema = "Application")]
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
