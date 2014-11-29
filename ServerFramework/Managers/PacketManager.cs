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
    public sealed class PacketManager : SingletonBase<PacketManager>
    {
        #region Fields

        private Dictionary<ushort, PacketHandler> _packetHandlers 
            = new Dictionary<ushort, PacketHandler>();

        #endregion

        #region Properties

        public int PacketHandlersCount
        {
            get { return _packetHandlers.Count; }
        }

        #endregion

        #region Properties

        internal Dictionary<ushort, PacketHandler> PacketHandlers 
        {
            get { return _packetHandlers; }
            set { _packetHandlers = value; }
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
                                        typeof(PacketHandler), meth) as PacketHandler;
                        }
                    }
                }
            }

            LogManager.Log(LogType.Normal, "{0} packet handlers loaded", PacketHandlers.Count);

            base.Init();
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
                LogManager.Log(LogType.Error, "Opcode {0} doesn't have handler", packet.Header.Opcode);
        }

        #endregion

        #endregion
    }
}
