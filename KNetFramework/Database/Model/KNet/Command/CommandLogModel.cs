/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Command
{
	[Table("Command.Log", Schema = "KNet")]
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
