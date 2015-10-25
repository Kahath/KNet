/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerFramework.Database.Model.Application.Command
{
	[Table("Command.Level", Schema = "Application")]
	public class CommandLevelModel : EntityBase
	{
		#region Properties

		[Key]
		public int ID		{ get; set; }
		public string Name	{ get; set; }

		#endregion
	}
}
