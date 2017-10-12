/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Command
{
	[Table("Command.Level", Schema = "Application")]
	public class CommandLevelModel : EntityBase<int>
	{
		#region Properties

		public string Name	{ get; set; }

		#endregion
	}
}
