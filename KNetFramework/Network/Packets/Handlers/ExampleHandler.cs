/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using KNetFramework.Attributes.Core;
using KNetFramework.Enums;
using KNetFramework.Network.Session;
using System;

namespace KNetFramework.Network.Packets.Handlers
{
	public static class ExampleHandler
	{
		#region Handlers

		#region ExamplePacketHandler

		#region Version 1

		[Opcode(0x0000, "Kahath", 1, OpcodeTypes.NotUsed)]
		private static void ExamplePacketHandler(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.ReadString();
			byte exampleData = packet.ReadUInt8();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Write data
			Action<Packet> packetAction = (pck) =>
			{
				pck.WriteString("Example string data");
			};

			//Send data
			pClient.Send(0x0001, 0, 100, packetAction);
		}

		#endregion

		#region Version 2

		[Opcode(0x0000, "Kahath", 2, OpcodeTypes.NotUsed)]
		private static void ExamplePacketHandlerTwo(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.ReadString();
			byte exampleData = packet.ReadUInt8();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Write data
			Action<Packet> packetAction = (pck) =>
			{
				pck.WriteString("Example string data");
				pck.WriteBits(0x123, 8);

				pck.WriteBit(true);
				pck.WriteBit(false);
			};

			//Send data
			pClient.Send(0x0001, 100, packetAction);
		}

		#endregion

		#endregion

		#endregion
	}
}
