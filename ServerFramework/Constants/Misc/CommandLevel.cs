using System;

namespace ServerFramework.Constants.Misc
{
    [Flags]
    public enum CommandLevel : ushort
    {
        None                    = 0x0000,
        CommandLevelOne         = 0x0001,
        CommandLevelTwo         = 0x0002,
        CommandLevelThree       = 0x0004,
        CommandLevelFour        = 0x0008,
        CommandLevelFive        = 0x0010,
        CommandLevelSix         = 0x0020,
        CommandLevelSeven       = 0x0040,
        CommandLevelEight       = 0x0080,
        CommandLevelNine        = 0x0100,
        CommandLevelTen         = 0x0200,
    };
}
