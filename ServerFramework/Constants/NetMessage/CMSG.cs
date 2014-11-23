using System;

namespace ServerFramework.Constants.NetMessage
{
    public enum CMSG : ushort
    {
        #region Authentication
        Login                           = 0x0001,
        #endregion

        #region Game
        #region Movement
        Move = 0x1001,
        #endregion
        InRangeObjects = 0x1002,
        #endregion

        #region Misc
        Ping                            = 0x7FFF,
        #endregion
    };
}
