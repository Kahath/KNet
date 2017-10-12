/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Commands.Base;
using ServerFramework.Database.Base.Repository;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Command;
using ServerFramework.Enums;
using ServerFramework.Managers;
using System.Collections.Generic;
using System.Linq;

namespace ServerFramework.Database.Repository
{
	public class CommandRepository : RepositoryBase<CommandModel>
	{
		#region Properties
		
		public new ApplicationContext Context
		{
			get { return base.Context as ApplicationContext; }
		}

		#endregion

		#region Constructors

		public CommandRepository(ApplicationContext context)
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
				IEnumerable<CommandModel> subCommands = Manager.DatabaseMgr.Get<CommandModel>(
					Context, x => x.Where(y => y.Parent.ID == parent.ID && y.Active));

				if (command.SubCommands != null && command.SubCommands.Any())
				{
					foreach (Command c in command.SubCommands)
					{
						CommandModel commandModel = subCommands.FirstOrDefault(x => x.Name == c.Name);
						commandModel = commandModel ?? new CommandModel(c) { Parent = parent };

						Manager.DatabaseMgr.AddOrUpdate(Context, false, commandModel);
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

			IEnumerable<CommandModel> subCommands = Manager.DatabaseMgr.Get<CommandModel>(
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
