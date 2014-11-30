/*
 * This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;


namespace ServerFramework.Constants.Entities.Console.Misc
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
        /// Returns number of objects on stack.
        /// </summary>
        public int Count
        {
            get { return this._stackPool.Count; }
        }

        #endregion

        #region Methods

        #region Push

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

        #region Pop

        /// <summary>
        /// Pops object from stack.
        /// </summary>
        /// <returns>object</returns>
        public T Pop()
        {
            lock (_stackPool)
                return this._stackPool.Pop();
        }

        #endregion

        #endregion
    }
}
