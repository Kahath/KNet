/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Log
{
	[Table("Log", Schema = "Application")]
	public class LogModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID				{ get; set; }
		public string Message		{ get; set; }
		public int LogTypeID		{ get; set; }

		[ForeignKey("LogTypeID")]
		public LogTypeModel LogType { get; set; }

		#endregion
	}
}
