/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Commands.Base;
using KNetFramework.Database.Base.Repository;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Command;
using KNetFramework.Enums;
using KNetFramework.Managers;
using System.Collections.Generic;
using System.Linq;

namespace KNetFramework.Database.Repository
{
	public class CommandRepository : RepositoryBase<CommandModel>
	{
		#region Properties
		
		public new KNetContext Context
		{
			get { return base.Context as KNetContext; }
		}

		#endregion

		#region Constructors

		public CommandRepository(KNetContext context)
			: base(context)
		{

		}

		#endregion

		#region Methods

		#region UpdateSubCommands

		public void UpdateSubCommands(Command command, CommandModel parent)
		{
			if (parent != null)
			{
				IEnumerable<CommandModel> subCommands = Manager.DatabaseManager.Get<CommandModel>(
					Context, x => x.Where(y => y.Parent.ID == parent.ID && y.Active));

				if (command.SubCommands != null && command.SubCommands.Any())
				{
					foreach (Command c in command.SubCommands)
					{
						CommandModel commandModel = subCommands.FirstOrDefault(x => x.Name == c.Name);
						commandModel = commandModel ?? new CommandModel(c) { Parent = parent };

						Manager.DatabaseManager.AddOrUpdate(Context, false, commandModel);
						UpdateSubCommands(c, commandModel);
					}
				}
			}
		}

		#endregion

		#region UpdateCommandInfo
		
		public void UpdateCommandInfo(Command command, CommandModel commandModel)
		{
			if(command != null && commandModel != null)
			{
				command.Model = commandModel;
				command.CommandLevel = (CommandLevel)commandModel.CommandLevelID;
				command.Description = commandModel.Description;
			}

			IEnumerable<CommandModel> subCommands = Manager.DatabaseManager.Get<CommandModel>(
				Context, x => x.Where(y => y.ParentID == commandModel.ID && y.Active).ToList());

			if (command.SubCommands != null && command.SubCommands.Any())
			{
				foreach(CommandModel sc in subCommands)
				{
					Command c = command.SubCommands.FirstOrDefault(x => x.Name == sc.Name);

					if (c != null)
						UpdateCommandInfo(c, sc);
				}
			}
		}

		#endregion

		#endregion
	}
}
