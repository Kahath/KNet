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
using ServerFramework.Database;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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

        protected override void LogPacket(Packet packet)
        {
            PacketLogType logtype = packet.Stream is BinaryReader ? PacketLogType.CMSG : PacketLogType.SMSG;

            if (!((ServerConfig.PacketLogLevel & logtype) == logtype) ? true : false)
                return;

            Client pClient = Manager.SessionMgr.GetClientBySessionId(packet.SessionId);

            PacketLogModel packetLog = new PacketLogModel();
            
            if(pClient != null)
            {
                packetLog.IP = pClient.IP;

                if(pClient.Token != null)
                {
                    packetLog.ClientID = pClient.Token.ID;
                }
            }

            packetLog.Size = packet.Header.Size;
            packetLog.Opcode = packet.Header.Opcode;

            using (ApplicationContext context = new ApplicationContext())
            {
                packetLog.PacketLogTypeID = packet.Stream is BinaryReader ?
                    context.PacketLogType.First(x => x.ID == (int)PacketLogType.CMSG && x.Active).ID :
                    context.PacketLogType.First(x => x.ID == (int)PacketLogType.SMSG && x.Active).ID;
            }

            if (packet.Header.Size > 0)
            {
                packetLog.Message = packet.Stream is BinaryReader ?
                    BitConverter.ToString(packet.Message) :
                    BitConverter.ToString(packet.Message, 4);
            }

            PacketLog.Add(packetLog);

            if(PacketLog.Count > 1000)
            {
                using(ApplicationContext context = new ApplicationContext())
                {
                    context.PacketLog.AddRange(PacketLog);
                    context.SaveChanges();
                }

                PacketLog.Clear();
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
