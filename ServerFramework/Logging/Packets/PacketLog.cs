using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Constants.NetMessage;
using ServerFramework.Network.Packets;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Xml;

namespace ServerFramework.Logging.Packets
{
    public class PacketLog
    {
        string Path;
        XmlDocument doc;
        BlockingCollection<Packet> packetLog = new BlockingCollection<Packet>();

        public PacketLog()
        {

        }

        public void Init()
        {
            Path = DateTime.Now.ToString("yyyy_MM_dd") + "_PacketLog.xml";
            if (File.Exists(this.Path))
            {
                doc = new XmlDocument();
                doc.Load(Path);
                startLog(doc);
                return;
            }

            doc = new XmlDocument();
            XmlDeclaration declaration =
                doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(declaration, root);

            XmlElement element = doc.CreateElement(string.Empty,
                "PacketLog", string.Empty);
            doc.AppendChild(element);

            doc.Save(this.Path);

            startLog(doc);
            //doc = null;
        }

        private void startLog(XmlDocument doc)
        {
            new Thread(() =>
            {
                while (true)
                {
                    var log = packetLog.Take();
                    if (log != null)
                        Log(log, doc);
                }
            }).Start();
        }

        public void Enqueue(Packet packet)
        {
            packetLog.Add(packet);
        }

        public void Log(Packet packet, XmlDocument doc)
        {
            PacketLogType logtype = packet.GetStream is BinaryReader ? PacketLogType.CMSG : PacketLogType.SMSG;
            if (!((ServerConfig.PacketLogLevel & logtype) == logtype) ? true : false)
                return;

            /*if (Path == null)
                return;*/

            //do

            XmlElement packetElement = doc.CreateElement(string.Empty,
                "Packet", string.Empty);

            XmlElement packetTime = doc.CreateElement(string.Empty,
                "DateTime", string.Empty);
            packetTime.InnerText = DateTime.Now.ToShortDateString() +
                " " + DateTime.Now.ToLongTimeString();

            XmlElement packetSize = doc.CreateElement(string.Empty,
                "Size", string.Empty);
            packetSize.InnerText = packet.Header.Size.ToString();

            XmlElement packetOpcode = doc.CreateElement(string.Empty,
                "Opcode", string.Empty);
            packetOpcode.InnerText =
                packet.GetStream is BinaryReader ?
                "CMSG " + string.Format("0x{0} ({1})", ((ushort)packet.Header.Opcode).ToString("X4"),
                Enum.GetName(typeof(CMSG), packet.Header.Opcode)) :
                "SMSG " + string.Format("0x{0} ({1})", ((ushort)packet.Header.Opcode).ToString("X4"),
                Enum.GetName(typeof(SMSG), packet.Header.Opcode));

            XmlElement pacaketMessage = doc.CreateElement(string.Empty,
                "Message", string.Empty);
            pacaketMessage.InnerText = BitConverter.ToString(packet.Message);


            doc.DocumentElement.AppendChild(packetElement);
            packetElement.AppendChild(packetTime);
            packetElement.AppendChild(packetSize);
            packetElement.AppendChild(packetOpcode);
            packetElement.AppendChild(pacaketMessage);

            doc.Save(Path);
            //doc = null;
        }
    }
}
