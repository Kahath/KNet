/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Command
{
	[Table("Command.Log", Schema = "Application")]
	public class CommandLogModel : EntityBase<int>
	{
		#region Properties

		public int UserID			{ get; set; }
		public string UserName		{ get; set; }
		public string CommandName	{ get; set; }
		public int CommandID		{ get; set; }

		[ForeignKey("CommandID")]
		public CommandModel Command { get; set; }

		#endregion
	}
}
