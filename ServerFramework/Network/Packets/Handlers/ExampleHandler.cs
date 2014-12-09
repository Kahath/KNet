using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Network.Packets.Handlers
{
    public class ExampleHandler
    {
        [Opcode(0x0000, "Kahath", 1.0, OpcodeType.Test)]
        public static void ExamplePacketHandler(UserToken packet)
        {
            Client pClient = Manager.SessionMgr.GetClientBySessionID(packet.SessionId);

            //Read if packet has data
            string testName = packet.Read<string>();
            byte testData = packet.Read<byte>();

            //Process data
            Console.WriteLine(testName);

            //Send back if need
            //Get packet with opcode
            UserToken token = pClient.PrepareSend(0x0001);

            //Write data
            token.Write<string>("Test string data");

            //Send data
            pClient.Send(token);
        }
    }
}
