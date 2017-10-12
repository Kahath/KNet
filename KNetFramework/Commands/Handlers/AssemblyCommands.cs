/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Commands.Base;
using KNetFramework.Enums;
using KNetFramework.Managers;
using KNetFramework.Network.Session;

namespace KNetFramework.Commands.Handlers
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

			Manager.AssemblyManager.Load(path);

			return true;
		}

		#endregion

		#endregion

		#endregion
	}
}
