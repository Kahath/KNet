/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Commands.Base;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Opcode;
using KNetFramework.Enums;
using KNetFramework.Managers;
using KNetFramework.Network.Packets;
using KNetFramework.Network.Session;
using System;
using System.Linq;
using System.Reflection;

namespace KNetFramework.Commands.Handlers
{
	[Command("opcode", CommandLevel.Ten, "")]
	public class OpcodeCommands : CommandHandlerBase
	{
		#region Methods

		#region GetCommand

		protected override Command GetCommand()
		{
			Command retVal = null;

			Command[] ForceSubCommands =
			{
				new Command("both", CommandLevel.Ten, null, ForceTypeVersionHandler, ""),
				new Command("type", CommandLevel.Ten, null, ForceTypeHandler, ""),
				new Command("version", CommandLevel.Ten, null, ForceVersionHandler, ""),
			};

			Command[] OpcodeSubCommands =
			{
				new Command("force", CommandLevel.Ten, ForceSubCommands, null, "")
			};

			retVal = new Command(Name, Level, OpcodeSubCommands, null, Description);

			return retVal;
		}

		#endregion

		#region Handlers

		#region ForceVersionHandler

		private static bool ForceVersionHandler(Client client, params string[] args)
		{
			int code = Int32.Parse(args[0]);
			int version = Int32.Parse(args[1]);

			OpcodeModel opcode = Manager.DatabaseManager.Get<KNetContext, OpcodeModel>(x =>
				x.AsNoTracking().FirstOrDefault(y => y.Code == code && y.Version == version && y.Active));

			ChangeOpcode(opcode);

			return true;
		}

		#endregion

		#region ForceTypeHandler

		private static bool ForceTypeHandler(Client client, params string[] args)
		{
			int code = Int32.Parse(args[0]);
			int opcodeType = int.Parse(args[1]);

			OpcodeModel opcode = Manager.DatabaseManager.Get<KNetContext, OpcodeModel>(x =>
				x.AsNoTracking().Where(y => y.Code == code && y.TypeID == opcodeType && y.Active)
				.OrderByDescending(y => y.Version)
				.FirstOrDefault());

			ChangeOpcode(opcode);

			return true;
		}

		#endregion

		#region ForceTypeVersionHandler

		private static bool ForceTypeVersionHandler(Client c, params string[] args)
		{
			int code = Int32.Parse(args[0]);
			int version = Int32.Parse(args[1]);
			int opcodeType = int.Parse(args[2]);

			OpcodeModel opcode = Manager.DatabaseManager.Get<KNetContext, OpcodeModel>(x =>
				x.AsNoTracking().FirstOrDefault(y => y.Code == code && y.Version == version
					&& y.TypeID == opcodeType && y.Active));

			ChangeOpcode(opcode);

			return true;
		}

		#endregion

		#endregion

		#region ChangeOpcode

		private static void ChangeOpcode(OpcodeModel opcode)
		{
			if (opcode != null)
			{
				if (Manager.PacketManager.PacketHandlers.ContainsKey((ushort)opcode.Code))
				{
					MethodInfo method = Manager.AssemblyManager.GetMethod
						(
							opcode.AssemblyName,
							opcode.TypeName,
							opcode.MethodName,
							typeof(Client),
							typeof(Packet)
						);

					if (method != null)
					{
						Manager.PacketManager.PacketHandlers[(ushort)opcode.Code] = Delegate.CreateDelegate(typeof(OpcodeHandler), method) as OpcodeHandler;
					}
				}
			}
		}

		#endregion

		#endregion
	}
}
