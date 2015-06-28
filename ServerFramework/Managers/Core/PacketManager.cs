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

using ServerFramework.Configuration;
using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServerFramework.Managers.Core
{
	public sealed class PacketManager : ManagerBase<PacketManager>
	{
		#region Fields

		private Dictionary<ushort, OpcodeHandler> _packetHandlers
			= new Dictionary<ushort, OpcodeHandler>();

		#endregion

		#region Properties

		internal Dictionary<ushort, OpcodeHandler> PacketHandlers
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

		public event PacketEventHandler BeforePacketInvoke;

		#endregion

		#region Constructor

		PacketManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		internal override void Init()
		{
			Dictionary<ushort, KeyValuePair<OpcodeAttribute, MethodInfo>> temp
				= new Dictionary<ushort, KeyValuePair<OpcodeAttribute, MethodInfo>>();

			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().
				Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(OpcodeAttribute))))
			{
				foreach (Type type in a.GetTypes())
				{
					foreach (MethodInfo meth in type.GetMethods())
					{
						foreach (OpcodeAttribute attr in meth.GetCustomAttributes<OpcodeAttribute>())
						{
							if (attr != null)
							{
								if ((ServerConfig.OpcodeAllowLevel & attr.Type) == attr.Type)
								{
									if (!temp.ContainsKey(attr.Opcode))
										temp[attr.Opcode]
											= new KeyValuePair<OpcodeAttribute, MethodInfo>(attr, meth);
									else
										if (temp[attr.Opcode].Key.Version < attr.Version)
											temp[attr.Opcode] = new KeyValuePair<OpcodeAttribute, MethodInfo>(attr, meth);
								}
							}
						}
					}
				}
			}

			foreach (KeyValuePair<OpcodeAttribute, MethodInfo> keyval in temp.Values)
			{
				PacketHandlers[keyval.Key.Opcode] = Delegate.CreateDelegate(
					typeof(OpcodeHandler), keyval.Value) as OpcodeHandler;
			}

			Manager.LogMgr.Log(LogType.Normal, "{0} packet handlers loaded", PacketHandlersCount);
		}

		#endregion

		#region InvokeHandler

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
							PacketHandlers[packet.Header.Opcode].Invoke(pClient, packet);
						else
							throw new ArgumentNullException("pClient");
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
							,	packet.Header.Size, packet.Header.Opcode
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

		private void BeforePacketInvokeEvent(Packet packet)
		{
			if (BeforePacketInvoke != null)
				BeforePacketInvoke(packet, new EventArgs());
		}

		#endregion

		#endregion
	}
}
