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
using ServerFramework.Constants.Entities.Session;
using ServerFramework.Constants.Misc;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace ServerFramework.Managers.Core
{
    public sealed class PacketLogManager : PacketLogManagerBase<PacketLogManager>
    {
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
                PacketLog = new StringBuilder(ServerConfig.PacketLogSize * 1024 * 1024);
            }
            else
            {
                PacketLog = new StringBuilder(ServerConfig.PacketLogSize * 1024 * 1024);
                PacketLog.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                File.Create(this.Path);
            }

            Thread logThread = new Thread(() =>
            {
                while (true)
                {
                    var item = PacketLogQueue.Take();

                    if (item != null)
                    {
                        LogPacket(item);
                    }
                }
            });

            logThread.IsBackground = true;
            logThread.Start();
        }

        #endregion

        #region LogPacket

        protected override async void LogPacket(Packet packet)
        {
            PacketLogType logtype = packet.Stream is BinaryReader ? PacketLogType.CMSG : PacketLogType.SMSG;

            if (!((ServerConfig.PacketLogLevel & logtype) == logtype) ? true : false)
                return;

            Client pClient = Manager.SessionMgr.GetClientBySessionId(packet.SessionId);

            using (StringWriter sw = new StringWriter(PacketLog))
            {
                using (XmlTextWriter xtw = new XmlTextWriter(sw))
                {
                    xtw.WriteStartElement("Packet");
                    xtw.WriteElementString("DateTime", DateTime.Now.ToString());

                    if (pClient != null)
                    {
                        xtw.WriteElementString("IP", pClient.IP);

                        if (pClient.Token != null)
                            xtw.WriteElementString("Client", pClient.Token.ToString());
                    }

                    xtw.WriteElementString("Size", packet.Header.Size.ToString());

                    string opcode = packet.Stream is BinaryReader ?
                        "CMSG " + string.Format("0x{0}", ((ushort)packet.Header.Opcode).ToString("X4")) :
                        "SMSG " + string.Format("0x{0}", ((ushort)packet.Header.Opcode).ToString("X4"));

                    xtw.WriteElementString("Opcode", opcode);
                    xtw.WriteElementString("Message", BitConverter.ToString(packet.Message));
                    xtw.WriteEndElement();

                    if (PacketLog.Length >= PacketLog.Capacity)
                    {
                        using (StreamWriter writer = new StreamWriter(Path, true, Encoding.UTF8))
                            await writer.WriteAsync(PacketLog.ToString());

                        PacketLog.Clear();
                    }
                }
            }
        }

        #endregion

        #region Log

		internal override void Log(Packet packet)
        {
            PacketLogQueue.Add(packet);
        }

        #endregion

        #endregion
    }
}
