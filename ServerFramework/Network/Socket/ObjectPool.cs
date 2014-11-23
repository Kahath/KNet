using System;
using System.Collections.Generic;


namespace ServerFramework.Network.Socket
{
    public class ObjectPool<T>
    {
        #region Fields

        private Stack<T> _stackPool;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new stack for objects
        /// </summary>
        /// <param name="capacity">capacity of stack</param>
        public ObjectPool(int capacity)
        {
            _stackPool = new Stack<T>(capacity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns object from stack.
        /// </summary>
        /// <returns>object</returns>
        public T Pop()
        {
            lock (_stackPool)
                return this._stackPool.Pop();
        }

        /// <summary>
        /// Returns number of objects on stack.
        /// </summary>
        public int Count
        {
            get { return this._stackPool.Count; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pushes item onto stack
        /// </summary>
        /// <param name="item">object</param>
        public void Push(T item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cannot be null");
            lock (_stackPool)
                this._stackPool.Push(item);
        }

        #endregion
    }
}
