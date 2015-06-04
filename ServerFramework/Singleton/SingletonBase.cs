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

using ServerFramework.Constants.Misc;
using ServerFramework.Managers;
using System;
using System.Globalization;
using System.Reflection;

namespace ServerFramework.Singleton
{
	public abstract class SingletonBase<T> where T : class
	{
		#region Fields

		private static T _instance;
		private static object _syncObject = new object();

		#endregion

		#region Methods

		#region GetInstance

		/// <summary>
		/// Gets instance of type T
		/// </summary>
		/// <param name="args">constructor parameters</param>
		/// <returns>new instance or already initialized instance</returns>
		public static T GetInstance(params object[] args)
		{
			object ctor = null;

			if (_instance == null)
			{
				lock (_syncObject)
				{
					if (_instance == null)
					{
						try
						{
							ctor = Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic
							, null, args, CultureInfo.CurrentCulture);
						}
						catch (Exception e)
						{
							Manager.LogMgr.Log(LogType.Error, "Error with creating instance of {0} type", typeof(T));
							Manager.LogMgr.Log(LogType.Error, "{0}", e.InnerException.ToString());
							Console.ReadLine();
							Environment.Exit(0);
						}

						_instance = ctor as T;

						return _instance;
					}
				}
			}

			return _instance;
		}

		#endregion

		#region Init

		internal abstract void Init();

		#endregion

		#endregion
	}
}
