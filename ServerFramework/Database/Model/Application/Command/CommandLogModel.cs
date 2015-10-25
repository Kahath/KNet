/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Command
{
	[Table("Command.Log", Schema = "Application")]
	public class CommandLogModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID				{ get; set; }
		public int UserID			{ get; set; }
		public string UserName		{ get; set; }
		public string CommandName	{ get; set; }
		public int CommandID		{ get; set; }

		[ForeignKey("CommandID")]
		public CommandModel Command { get; set; }

		#endregion
	}
}
