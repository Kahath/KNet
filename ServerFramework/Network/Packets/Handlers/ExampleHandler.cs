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
