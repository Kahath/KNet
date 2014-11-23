using System;

namespace ServerFramework.Constants.Misc
{
    public enum OpcodeType : byte
    {
        Test                    = 0x00,
        Broken                  = 0x01,
        Finished                = 0x02,
        NotUsed                 = 0x03,
        InDevelopment           = 0x04,
    };
}
