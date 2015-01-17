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
using ServerFramework.Constants.Attributes;
using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Network.Packets;
using ServerFramework.Singleton;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServerFramework.Managers
{
    internal sealed class PacketManager : SingletonBase<PacketManager>
    {
        #region Fields

        private Dictionary<ushort, PacketHandler> _packetHandlers 
            = new Dictionary<ushort, PacketHandler>();

        #endregion

        #region Properties

        internal Dictionary<ushort, PacketHandler> PacketHandlers 
        {
            get { return _packetHandlers; }
            set { _packetHandlers = value; }
        }

        public int PacketHandlersCount
        {
            get { return _packetHandlers.Count; }
        }

        #endregion

        #region Events

        public event PacketManagerInvokeEventHandler BeforePacketInvoke;

        #endregion

        #region Constructor

        PacketManager()
        {
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal override void Init()
        {
            Dictionary<ushort, KeyValuePair<OpcodeAttribute, MethodInfo>> temp 
                = new Dictionary<ushort, KeyValuePair<OpcodeAttribute, MethodInfo>>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in a.GetTypes())
                {
                    foreach (var meth in type.GetMethods())
                    {
                        foreach (OpcodeAttribute attr in meth.GetCustomAttributes<OpcodeAttribute>())
                        {
                            if (attr != null)
                            {
                                if ((ServerConfig.OpcodeAllowLevel & attr.Type) == attr.Type)
                                {
                                    if (!temp.ContainsKey(attr.Opcode))
                                        temp[attr.Opcode]
                                            = new KeyValuePair<OpcodeAttribute, MethodInfo>(attr, meth);
                                    else
                                        if(temp[attr.Opcode].Key.Version < attr.Version)
                                            temp[attr.Opcode] = new KeyValuePair<OpcodeAttribute, MethodInfo>(attr, meth);
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<OpcodeAttribute, MethodInfo> keyval in temp.Values)
            {
                PacketHandlers[keyval.Key.Opcode] = Delegate.CreateDelegate(
                    typeof(PacketHandler), keyval.Value) as PacketHandler;
            }

            LogManager.Log(LogType.Normal, "{0} packet handlers loaded", PacketHandlersCount);

            base.Init();
        }

        #endregion

        #region InvokeHandler

        internal void InvokeHandler(Packet packet)
        {
            if (BeforePacketInvoke != null)
                BeforePacketInvoke(packet, new EventArgs());

            if (PacketHandlers.ContainsKey(packet.Header.Opcode))
            {
                try
                {
                    PacketHandlers[packet.Header.Opcode].Invoke(packet);
                }
                catch(Exception)
                {
                    OpcodeAttribute attr =
                        PacketHandlers[packet.Header.Opcode].
                        GetMethodInfo().GetCustomAttribute(typeof(OpcodeAttribute))
                        as OpcodeAttribute;

                    if (attr != null)
                    {
                        LogManager.Log(LogType.Error, "Error with '0x{0:X}' opcode"
                            + " authored by '{1}' using version '{2}' and type '{3}'"
                            , attr.Opcode, attr.Author, attr.Version, attr.Type);

                        LogManager.Log(LogType.Error, "Packet size: {0}"
                            , packet.Header.Size);
                        LogManager.Log(LogType.Error, "Packet opcode: {0:X}"
                            , packet.Header.Opcode);
                        LogManager.Log(LogType.Error, "Packet content: {0}"
                            , BitConverter.ToString(packet.Message));
                    }
                }
            }
            else
                LogManager.Log(LogType.Error, "Opcode 0x{0:X} doesn't have handler", packet.Header.Opcode);
        }

        #endregion

        #endregion
    }
}
