/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Commands.Base;
using ServerFramework.Enums;
using ServerFramework.Managers;
using ServerFramework.Network.Session;

namespace ServerFramework.Commands.Handlers
{
	[Command("assembly", CommandLevel.Ten, "")]
	public class AssemblyCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			Command[] AssemblyCommands =
			{
				new Command("load", CommandLevel.Ten, null, AssemblyLoadHandler, ""),
			};

			retVal = new Command(Name, Level, AssemblyCommands, null, Description);
				
			return retVal;
		}

		#endregion

		#region Handlers

		#region AssemblyLoadHandler

		private static bool AssemblyLoadHandler(Client client, params string[] args)
		{
			string path = args[0];

			Manager.AssemblyMgr.Load(path);

			return true;
		}

		#endregion

		#endregion

		#endregion
	}
}
