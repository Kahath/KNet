
namespace ServerFramework.Network.Packets
{
    public class PacketHeader
    {
        #region Fields

        private ushort _size;
        private ushort _opcode;

        #endregion

        #region Properties

        public ushort Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public ushort Opcode
        {
            get { return _opcode; }
            set { _opcode = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of packet header
        /// which contains size and opcode
        /// </summary>
        public PacketHeader()
        {

        }

        #endregion
    }
}
