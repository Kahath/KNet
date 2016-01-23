/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Enums;
using ServerFramework.Managers;
using System;
using System.Linq;
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
		/// Gets instance of generic type.
		/// </summary>
		/// <param name="args">Constructor parameters.</param>
		/// <returns>New instance or already initialized instance.</returns>
		public static T GetInstance(params object[] args)
		{
			ConstructorInfo ctor;
			object obj = null;

			if (_instance == null)
			{
				lock (_syncObject)
				{
					if (_instance == null)
					{
						try
						{
							Type[] types = Type.EmptyTypes;

							if(args != null && args.Any())
								types = args.Select(x => x.GetType()).ToArray();

							ctor = typeof(T).GetConstructor
							(
								BindingFlags.Instance | BindingFlags.NonPublic
							,	null
							,	types
							,	null
							);

							obj = ctor.Invoke(args);
						}
						catch (Exception e)
						{
							Manager.LogMgr.Log
							(
								LogType.Critical
							,	$"Error with creating instance of {typeof(T)} type\n{e.ToString()}"
							);

							Console.ReadLine();
						}

						_instance = obj as T;
					}
				}
			}

			return _instance;
		}

		#endregion

		#endregion
	}
}
