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

		[Opcode(0x0000, "Kahath", 1, OpcodeType.Test)]
		private static void ExamplePacketHandler(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.Read<string>();
			byte exampleData = packet.Read<byte>();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Create new packet for send
			using (packet = new Packet(0x0001, Encoding.UTF8, (byte)PacketFlag.Log))
			{
				//Write data
				packet.Write<string>("Example string data");

				//Send data
				pClient.Send(packet);
			}
		}

		#endregion

		#region Version 2

		[Opcode(0x0000, "Kahath", 2, OpcodeType.Test)]
		private static void ExamplePacketHandlerTwo(Client pClient, Packet packet)
		{
			//Read if packet has data
			string exampleName = packet.Read<string>();
			byte exampleData = packet.Read<byte>();

			//Process data
			Console.WriteLine(exampleName);

			//Send back if need
			//Create new packet for send
			using (packet = new Packet(0x0001, Encoding.UTF8))
			{
				//Write data
				packet.Write<string>("Example string data");

				//Write bits
				packet.WriteBits<int>(0x123, 8);

				packet.WriteBit(true);
				packet.WriteBit(false);

				//Send data
				pClient.Send(packet);
			}
		}

		#endregion

		#endregion

		#endregion
	}
}
