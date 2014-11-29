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

        private Dictionary<ushort, HandlePacket> _packetHandlers 
            = new Dictionary<ushort, HandlePacket>();

        #endregion

        #region Properties

        public Dictionary<ushort, HandlePacket> PacketHandlers 
        {
            get { return _packetHandlers; }
            set { _packetHandlers = value; }
        }

        #endregion

        #region Delegates

        public delegate void HandlePacket(Packet packet);
        public delegate void PacketManagerInvokeHandler(object sender, EventArgs e);

        #endregion

        #region Events

        public event PacketManagerInvokeHandler BeforePacketInvoke;
        public event PacketManagerInvokeHandler AfterInitialisation;

        #endregion

        #region Constructor

        PacketManager()
        {
            Init();
        }

        #endregion

        #region Methods

        #region Init

        internal void Init()
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in a.GetTypes())
                {
                    foreach (var meth in type.GetMethods())
                    {
                        foreach (OpcodeAttribute attr in meth.GetCustomAttributes<OpcodeAttribute>())
                        {
                            if (attr != null)
                                if (!PacketHandlers.ContainsKey(attr.Opcode))
                                    PacketHandlers[attr.Opcode] = Delegate.CreateDelegate(
                                        typeof(HandlePacket), meth) as HandlePacket;
                        }
                    }
                }
            }

            Log.Message(LogType.Normal, "{0} packet handlers loaded", PacketHandlers.Count);

            if (AfterInitialisation != null)
                AfterInitialisation(PacketHandlers, new EventArgs());
        }

        #endregion

        #region InvokeHandler

        internal void InvokeHandler(Packet packet)
        {
            if (BeforePacketInvoke != null)
                BeforePacketInvoke(packet, new EventArgs());

            if (PacketHandlers.ContainsKey(packet.Header.Opcode))
                PacketHandlers[packet.Header.Opcode].Invoke(packet);
            else
                Log.Message(LogType.Error, "Opcode {0} doesn't have handler", packet.Header.Opcode);
        }

        #endregion

        #endregion
    }
}
