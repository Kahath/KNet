/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using ServerFramework.Configuration.Base;

namespace ServerFramework.Configuration.Core
{
	/// <summary>
	/// Provides configuration dependency injection.
	/// </summary>
	public class Config : Dependency<IConfig>
	{
		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Configuration.Config"/> type.
		/// </summary>
		/// <param name="path">Path to configuration file.</param>
		public Config(string path)
			: base(ResolveType.Transient, path)
		{

		}

		#endregion

		#region Methods

		#region Read

		/// <summary>
		/// Reads generic value from configuration.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="config">Name of configuration.</param>
		/// <param name="hex">Is value written as hexadecimal value.</param>
		/// <returns>Configuration value of generic data type.</returns>
		public T Read<T>(string config, bool hex = false)
		{
			return Instance.Read<T>(config, hex);
		}

		#endregion

		#endregion
	}
}
