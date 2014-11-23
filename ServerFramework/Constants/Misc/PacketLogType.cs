using System;

namespace ServerFramework.Constants.Misc
{
    [Flags]
    enum PacketLogType : byte
    {
        None    = 0x00,
        SMSG    = 0x01,
        CMSG    = 0x02,
    };
}
