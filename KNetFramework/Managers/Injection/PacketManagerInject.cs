/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Database.Context;
using KNetFramework.Database.Model.KNet.Opcode;
using KNetFramework.Enums;
using KNetFramework.Managers.Interface;
using KNetFramework.Network.Packets;
using KNetFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KNetFramework.Managers.Injection
{
	public sealed class PacketManagerInject : IPacketManager
	{
		#region Fields

		private ConcurrentDictionary<ushort, OpcodeHandler> _packetHandlers = new ConcurrentDictionary<ushort, OpcodeHandler>();

		#endregion

		#region Properties

		public ConcurrentDictionary<ushort, OpcodeHandler> PacketHandlers
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
		/// Creates new Instance of <see cref="KNetFramework.Managers.Core.PacketManager"/> type.
		/// </summary>
		PacketManagerInject()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises PacketManager.
		/// </summary>
		public void Init()
		{
			IEnumerable<OpcodeModel> opcodes = Manager.DatabaseManager.Get<KNetContext, OpcodeModel>(x =>
				x.AsNoTracking().Where(y => y.Active)
				.GroupBy(y => y.Code, (key, y) =>
					y.OrderByDescending(z => z.TypeID)
					.ThenByDescending(z => z.Version)
					.FirstOrDefault()).ToList());

			foreach (OpcodeModel opcode in opcodes)
			{
				PacketHandlers[(ushort)opcode.Code] = Delegate.CreateDelegate
				(
					typeof(OpcodeHandler)
				,	Manager.AssemblyManager.GetMethod
					(
						opcode.AssemblyName
					,	opcode.TypeName
					,	opcode.MethodName
					,	typeof(Client)
					,	typeof(Packet)
					)
				) as OpcodeHandler;
			}

			Manager.LogManager.Log(LogTypes.Normal, $"{PacketHandlersCount} packet handlers loaded");
		}

		#endregion

		#region InvokeHandler

		/// <summary>
		/// Invokes packet script.
		/// </summary>
		/// <param name="packet">Instance of <see cref="KNetFramework.Network.Packets.Packet"/> type.</param>
		public void InvokeHandler(Packet packet)
		{
			BeforePacketInvokeEvent(packet);

			if (PacketHandlers.ContainsKey(packet.Header.Opcode))
			{
				try
				{
					Client pClient = Manager.SessionManager.GetClient(packet.SessionID);

					if (pClient != null)
						PacketHandlers[packet.Header.Opcode](pClient, packet);
				}
				catch (Exception e)
				{

					if (PacketHandlers[packet.Header.Opcode].GetMethodInfo().GetCustomAttribute(typeof(OpcodeAttribute)) is OpcodeAttribute attr)
					{
						Manager.LogManager.Log
							(
								LogTypes.Warning
							, $"Error with '0x{attr.Opcode:X}' opcode "
							+ $"authored by '{attr.Author}' using version '{attr.Version}' and type '{attr.Type}'\n"
							+ $"Packet size: {packet.Header.Length}\nPacket opcode: 0x{packet.Header.Opcode:X}"
							+ $"\nPacket content: {BitConverter.ToString(packet.ToArray())}"
							);
					}

					Manager.LogManager.Log(LogTypes.Error, e);
				}
			}
			else
			{
				Manager.LogManager.Log(LogTypes.Warning, $"Opcode 0x{packet.Header.Opcode:X} doesn't have handler");
			}
		}

		#endregion

		#region BeforePacketInvokeEvent

		/// <summary>
		/// Handles BeforePacketInvoke event.
		/// </summary>
		/// <param name="packet">Instance of <see cref="KNetFramework.Network.Packets.Packet"/> type.</param>
		private void BeforePacketInvokeEvent(Packet packet)
		{
			BeforePacketInvoke?.Invoke(packet, new EventArgs());
		}

		#endregion

		#endregion
	}
}
