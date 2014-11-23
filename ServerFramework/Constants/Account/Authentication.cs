
namespace ServerFramework.Constants.Account
{
    public enum Authentication : byte
    {
        Null                    = 0x00,
        Correct                 = 0x01,
        FailWrongInfo           = 0x02,
        FailAlreadyOnline       = 0x03,
    };
}
