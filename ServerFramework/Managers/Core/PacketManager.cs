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

using ServerFramework.Attributes.Core;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.Opcode;
using ServerFramework.Enums;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Managers.Core
{
	public sealed class PacketManager : ManagerBase<PacketManager>
	{
		#region Fields

		private ConcurrentDictionary<ushort, OpcodeHandler> _packetHandlers
			= new ConcurrentDictionary<ushort, OpcodeHandler>();

		#endregion

		#region Properties

		internal ConcurrentDictionary<ushort, OpcodeHandler> PacketHandlers
		{
			get { return _packetHandlers; }
			set { _packetHandlers = value; }
		}

		public int PacketHandlersCount
		{
			get { return _packetHandlers.Count; }
		}

		#endregion

		#region Events

		public event EventHandler BeforePacketInvoke;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates new Instance of <see cref="ServerFramework.Managers.Core.PacketManager"/> type.
		/// </summary>
		PacketManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises PacketManager.
		/// </summary>
		protected override void Init()
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				IEnumerable<OpcodeModel> opcodes = context.Opcodes
					.Where(x => x.Active)
					.GroupBy(x => x.Code, (key, y) => 
						y.OrderByDescending(x => x.TypeID)
						.ThenByDescending(x => x.Version)
						.FirstOrDefault());

				foreach (OpcodeModel opcode in opcodes)
				{
					PacketHandlers[(ushort)opcode.Code] = Delegate.CreateDelegate
					(
						typeof(OpcodeHandler)
					,	Manager.AssemblyMgr.GetMethod
						(
							opcode.AssemblyName
						,	opcode.TypeName
						,	opcode.MethodName
						,	typeof(Client)
						,	typeof(Packet)
						)
					) as OpcodeHandler;
				}
			}

			Manager.LogMgr.Log(LogType.Normal, "{0} packet handlers loaded", PacketHandlersCount);
		}

		#endregion

		#region InvokeHandler

		/// <summary>
		/// Invokes packet script.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		internal void InvokeHandler(Packet packet)
		{
			BeforePacketInvokeEvent(packet);

			if (PacketHandlers.ContainsKey(packet.Header.Opcode))
			{
				try
				{
					using (packet)
					{
						Client pClient = Manager.SessionMgr.GetClientBySession(packet.SessionId);

						if (pClient != null)
							PacketHandlers[packet.Header.Opcode](pClient, packet);
					}
				}
				catch (Exception e)
				{
					OpcodeAttribute attr =
						PacketHandlers[packet.Header.Opcode].
						GetMethodInfo().GetCustomAttribute(typeof(OpcodeAttribute))
						as OpcodeAttribute;

					if (attr != null)
					{
						Manager.LogMgr.Log
							(
								LogType.Warning
							,	"Error with '0x{0:X}' opcode "
									+ "authored by '{1}' using version '{2}' and type '{3}'\n"
									+ "Packet size: {4}\nPacket opcode: 0x{5:X}\nPacket content: {6}"
							,	attr.Opcode, attr.Author, attr.Version, attr.Type
							,	packet.Header.Length, packet.Header.Opcode
							,	BitConverter.ToString(packet.Message)
							);
					}

					Manager.LogMgr.Log(LogType.Error, "{0}", e.ToString());
				}
			}
			else
			{
				Manager.LogMgr.Log(LogType.Warning, "Opcode 0x{0:X} doesn't have handler", packet.Header.Opcode);
			}
		}

		#endregion

		#region BeforePacketInvokeEvent

		/// <summary>
		/// Handles BeforePacketInvoke event.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		private void BeforePacketInvokeEvent(Packet packet)
		{
			if (BeforePacketInvoke != null)
				BeforePacketInvoke(packet, new EventArgs());
		}

		#endregion

		#endregion
	}
}
