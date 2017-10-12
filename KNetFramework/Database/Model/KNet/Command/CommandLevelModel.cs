/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Command
{
	[Table("Command.Level", Schema = "KNet")]
	public class CommandLevelModel : EntityBase<int>
	{
		#region Properties

		public string Name	{ get; set; }

		#endregion
	}
}
