using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Network.Packets;
using ServerFramework.Singleton;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Xml;

namespace ServerFramework.Logging.Packets
{
    public class PacketLogManager : SingletonBase<PacketLogManager>
    {
        #region Fields

        private string _path;
        private XmlDocument _doc;
        private BlockingCollection<Packet> _packetLogQueue
            = new BlockingCollection<Packet>();

        #endregion

        #region Properties

        internal string Path
        { 
            get { return _path; }
            set { _path = value; }
        }

        internal XmlDocument Doc
        {
            get { return _doc; }
            set { _doc = value; }
        }

        internal BlockingCollection<Packet> PacketLogQueue
        {
            get { return _packetLogQueue; }
            set { _packetLogQueue = value; }
        }


        #endregion

        #region Constructors

        PacketLogManager()
        {
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal override void Init()
        {
            Path = DateTime.Now.ToString("yyyy_MM_dd") + "_PacketLog.xml";
            if (File.Exists(this.Path))
            {
                Doc = new XmlDocument();
                Doc.Load(this.Path);
            }
            else
            {
                Doc = new XmlDocument();
                XmlDeclaration declaration =
                    Doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlElement root = Doc.DocumentElement;
                Doc.InsertBefore(declaration, root);

                XmlElement element = Doc.CreateElement(string.Empty,
                    "PacketLog", string.Empty);
                Doc.AppendChild(element);

                Doc.Save(this.Path);     
            }

            base.Init();

            Thread logThread = new Thread(() =>
            {
                while (true)
                {
                    var item = _packetLogQueue.Take();

                    if (item != null)
                        LogPacket(item);
                }
            });

            logThread.IsBackground = true;
            logThread.Start();
        }

        #endregion

        #region LogPacket

        private void LogPacket(Packet packet)
        {
            PacketLogType logtype = packet.GetStream is BinaryReader ? PacketLogType.CMSG : PacketLogType.SMSG;
            if (!((ServerConfig.PacketLogLevel & logtype) == logtype) ? true : false)
                return;

            XmlElement packetElement = Doc.CreateElement(string.Empty,
                "Packet", string.Empty);

            XmlElement packetTime = Doc.CreateElement(string.Empty,
                "DateTime", string.Empty);
            packetTime.InnerText = DateTime.Now.ToShortDateString() +
                " " + DateTime.Now.ToLongTimeString();

            XmlElement packetSize = Doc.CreateElement(string.Empty,
                "Size", string.Empty);
            packetSize.InnerText = packet.Header.Size.ToString();

            XmlElement packetOpcode = Doc.CreateElement(string.Empty,
                "Opcode", string.Empty);
            packetOpcode.InnerText =
                packet.GetStream is BinaryReader ?
                "CMSG " + string.Format("0x{0}", ((ushort)packet.Header.Opcode).ToString("X4")
                //, Enum.GetName(typeof(CMSG), packet.Header.Opcode)
                    ) :
                "SMSG " + string.Format("0x{0}", ((ushort)packet.Header.Opcode).ToString("X4")
                //, Enum.GetName(typeof(SMSG), packet.Header.Opcode)
                    );

            XmlElement pacaketMessage = Doc.CreateElement(string.Empty,
                "Message", string.Empty);
            pacaketMessage.InnerText = BitConverter.ToString(packet.Message);


            Doc.DocumentElement.AppendChild(packetElement);
            packetElement.AppendChild(packetTime);
            packetElement.AppendChild(packetSize);
            packetElement.AppendChild(packetOpcode);
            packetElement.AppendChild(pacaketMessage);

            Doc.Save(Path);
        }

        #endregion

        #region Log

        internal void Log(Packet packet)
        {
            PacketLogQueue.Add(packet);
        }

        #endregion

        #endregion
    }
}
