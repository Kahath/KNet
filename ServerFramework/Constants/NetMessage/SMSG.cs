using System;

namespace ServerFramework.Constants.NetMessage
{
    public enum SMSG : ushort
    {
        #region Authentication
        LoginResponse                   = 0x0001,
        #endregion

        #region Misc
        Pong                            = 0x7FFF,
        #endregion

        #region Game

        #region Movement
        LoginLocation = 0x1000,
        Move = 0x1001,
        #endregion 

        InRangeObjects = 0x1002,

        #endregion
    };
}
