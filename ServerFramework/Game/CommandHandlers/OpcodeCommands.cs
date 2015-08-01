/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Console;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Opcode;
using ServerFramework.Managers;
using System;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Game.CommandHandlers
{
	[Command]
	public static class OpcodeCommands
	{
		#region Methods

		#region GetCommand

		private static Command GetCommand()
		{
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

			return new Command("opcode", CommandLevel.Ten, OpcodeSubCommands, null, "");
		}

		#endregion

		#region Handlers

		#region ForceVersionHandler

		private static bool ForceVersionHandler(Client client, params string[] args)
		{
			int code = int.Parse(args[0]);
			int version = int.Parse(args[1]);

			using(ApplicationContext context = new ApplicationContext())
			{
				OpcodeModel opcode = context.Opcodes.FirstOrDefault(x => x.Code == code && x.Version == version && x.Active);

				if(opcode != null)
				{
					if(Manager.PacketMgr.PacketHandlers.ContainsKey((ushort)opcode.Code))
					{
						MethodInfo method = Manager.AssemblyMgr.GetMethod(opcode.AssemblyName, opcode.TypeName, opcode.MethodName);

						if (method != null)
						{
							Manager.PacketMgr.PacketHandlers[(ushort)opcode.Code]
								= Delegate.CreateDelegate(typeof(OpcodeHandler), method) as OpcodeHandler;
						}
					}
				}
			}

			return true;
		}

		#endregion

		#region ForceTypeHandler

		private static bool ForceTypeHandler(Client client, params string[] args)
		{
			int code = int.Parse(args[0]);
			int opcodeType = (int)Enum.Parse(typeof(OpcodeType), args[1]);

			using (ApplicationContext context = new ApplicationContext())
			{
				OpcodeModel opcode = context.Opcodes
					.Where(x => x.Code == code && x.TypeID == opcodeType && x.Active)
					.OrderByDescending(x => x.Version)
					.FirstOrDefault();

				if (opcode != null)
				{
					if (Manager.PacketMgr.PacketHandlers.ContainsKey((ushort)opcode.Code))
					{
						MethodInfo method = Manager.AssemblyMgr.GetMethod(opcode.AssemblyName, opcode.TypeName, opcode.MethodName);

						if (method != null)
						{
							Manager.PacketMgr.PacketHandlers[(ushort)opcode.Code]
								= Delegate.CreateDelegate(typeof(OpcodeHandler), method) as OpcodeHandler;
						}
					}
				}
			}


			return true;
		}

		#endregion

		#region ForceTypeVersionHandler

		private static bool ForceTypeVersionHandler(Client c, params string[] args)
		{
			int code = int.Parse(args[0]);
			int version = int.Parse(args[1]);
			int opcodeType = (int)Enum.Parse(typeof(OpcodeType), args[1]);

			using(ApplicationContext context = new ApplicationContext())
			{
				OpcodeModel opcode = context.Opcodes.FirstOrDefault(x => x.Code == code && x.Version == version && x.TypeID == opcodeType);

				if(opcode != null)
				{
					if (Manager.PacketMgr.PacketHandlers.ContainsKey((ushort)opcode.Code))
					{
						MethodInfo method = Manager.AssemblyMgr.GetMethod(opcode.AssemblyName, opcode.TypeName, opcode.MethodName);

						if (method != null)
						{
							Manager.PacketMgr.PacketHandlers[(ushort)opcode.Code]
							= Delegate.CreateDelegate(typeof(OpcodeHandler), method) as OpcodeHandler;
						}
					}
				}
			}

			return true;
		}

		#endregion

		#endregion

		#endregion
	}
}
