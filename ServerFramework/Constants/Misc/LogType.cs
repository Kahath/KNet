using System;

namespace ServerFramework.Constants.Misc
{
    [Flags]
    public enum LogType : byte
    {
        None        = 0x00,
        Normal      = 0x01,
        Error       = 0x02,
        Init        = 0x04,
        Database    = 0x08,
        Debug       = 0x10,
        Dump        = 0x20,
        Cmd         = 0x40,
        Default     = 0x80,
    };
}
