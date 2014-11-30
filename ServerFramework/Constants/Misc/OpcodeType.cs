using System;

namespace ServerFramework.Constants.Misc
{
    [Flags]
    public enum OpcodeType : byte
    {
        Test                    = 0x01,
        Broken                  = 0x02,
        Finished                = 0x04,
        NotUsed                 = 0x08,
        InDevelopment           = 0x10,
    };
}
