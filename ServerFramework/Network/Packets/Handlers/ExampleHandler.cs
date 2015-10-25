/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Attributes.Core;
using ServerFramework.Enums;
using ServerFramework.Network.Session;
using System;
using System.Text;

namespace ServerFramework.Network.Packets.Handlers
{
	public static class ExampleHandler
	{
		#region Handlers

		#region ExamplePacketHandler

		#region Version 1

		[Opcode(0x0000, "Kahath", 1, OpcodeType.NotUsed)]
		private static void ExamplePacketHandler(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.Read<string>();
			byte exampleData = packet.Read<byte>();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Write data
			Action<Packet> packetAction = (pck) =>
			{
				pck.Write("Example string data");
			};

			//Send data
			pClient.Send(0x0001, 0, 100, packetAction);
		}

		#endregion

		#region Version 2

		[Opcode(0x0000, "Kahath", 2, OpcodeType.NotUsed)]
		private static void ExamplePacketHandlerTwo(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.Read<string>();
			byte exampleData = packet.Read<byte>();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Write data
			Action<Packet> packetAction = (pck) =>
			{
				pck.Write("Example string data");
				pck.WriteBits(0x123, 8);

				pck.WriteBit(true);
				pck.WriteBit(false);
			};

			//Send data
			pClient.Send(0x0001, 0, 100, packetAction);
		}

		#endregion

		#endregion

		#endregion
	}
}
