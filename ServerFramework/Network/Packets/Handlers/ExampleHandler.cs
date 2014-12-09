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
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers;
using System;

namespace ServerFramework.Network.Packets.Handlers
{
    public class ExampleHandler
    {
        [Opcode(0x0000, "Kahath", 1.0, OpcodeType.Test)]
        public static void ExamplePacketHandler(UserToken packet)
        {
            Client pClient = Manager.SessionMgr.GetClientBySessionID(packet.SessionId);

            //Read if packet has data
            string exampleName = packet.Read<string>();
            byte exampleData = packet.Read<byte>();

            //Process data
            Console.WriteLine(exampleName);

            //Send back if need
            //Get packet with opcode
            UserToken token = pClient.PrepareSend(0x0001);

            //Write data
            token.Write<string>("Example string data");

            //Send data
            pClient.Send(token);
        }
    }
}
