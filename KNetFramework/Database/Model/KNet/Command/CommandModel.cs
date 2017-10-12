/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Commands.Base;
using KNetFramework.Database.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KNetFramework.Database.Model.KNet.Command
{
	[Table("Command", Schema = "KNet")]
	public class CommandModel : AssemblyEntityBase<int>
	{
		#region Properties


		[StringLength(50)]
		public string Name						{ get; set; }
		public string Description				{ get; set; }
		public int? CommandLevelID				{ get; set; }
		public int? ParentID					{ get; set; }

		[ForeignKey("CommandLevelID")]
		public CommandLevelModel CommandLevel	{ get; set; }

		[ForeignKey("ParentID")]
		public CommandModel Parent				{ get; set; }

		#endregion

		#region Constructors

		public CommandModel()
		{

		}

		public CommandModel(CommandHandlerBase commandHandler)
		{
			Name = commandHandler.Name;
			Description = commandHandler.Description;
			CommandLevelID = (int)commandHandler.Level;
		}

		public CommandModel(KNetFramework.Commands.Base.Command command)
		{
			Name = command.Name;
			Description = command.Description;
			CommandLevelID = (int)command.CommandLevel;
		}

		#endregion
	}
}
