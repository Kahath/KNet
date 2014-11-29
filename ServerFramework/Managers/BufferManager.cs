using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using ServerFramework.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Managers
{
    internal sealed class BufferManager : SingletonBase<BufferManager>
    {
        #region Fields

        int totalBytesInBufferBlock;

        byte[] bufferBlock;
        Stack<int> freeIndexPool;
        int currentIndex;
        int bufferBytesAllocatedForEachSaea;

        #endregion

        #region Properties

        public int TotalBytes
        {
            get { return this.totalBytesInBufferBlock; }
        }

        #endregion

        #region Constructors

        BufferManager(int totalBytes, int totalBytesInEachSaeaObject)
        {
            this.totalBytesInBufferBlock = totalBytes;
            this.currentIndex = 0;
            this.bufferBytesAllocatedForEachSaea = totalBytesInEachSaeaObject;
            this.freeIndexPool = new Stack<int>();
            Init();
        }

        #endregion

        #region Methods

        #region Init

        public void Init()
        {
            this.bufferBlock = new byte[totalBytesInBufferBlock];
            Log.Message(LogType.Normal, "Buffer alocated size: {0}KB", this.TotalBytes / 1024);
        }

        #endregion

        #region SetBuffer

        public bool SetBuffer(SocketAsyncEventArgs e)
        {
            if (this.freeIndexPool.Count > 0)
            {
                e.SetBuffer(this.bufferBlock, this.freeIndexPool.Pop(),
                    this.bufferBytesAllocatedForEachSaea);
            }
            else
            {
                if ((this.totalBytesInBufferBlock - this.bufferBytesAllocatedForEachSaea) <
                    this.currentIndex)
                    return false;

                e.SetBuffer(this.bufferBlock, this.currentIndex,
                    this.bufferBytesAllocatedForEachSaea);

                this.currentIndex += this.bufferBytesAllocatedForEachSaea;
            }

            return true;
        }

        #endregion

        #region FreeBuffer

        public void FreeBuffer(SocketAsyncEventArgs e)
        {
            this.freeIndexPool.Push(e.Offset);
            e.SetBuffer(null, 0, 0);
        }

        #endregion

        #endregion
    }
}
