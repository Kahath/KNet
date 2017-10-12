/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using DILibrary.Constants;
using DILibrary.DependencyInjection;
using KNetFramework.Configuration.Base;

namespace KNetFramework.Configuration.Core
{
	/// <summary>
	/// Provides configuration dependency injection.
	/// </summary>
	public class Config : Dependency<Config, IConfig>
	{
		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="KNetFramework.Configuration.Config"/> type.
		/// </summary>
		/// <param name="path">Path to configuration file.</param>
		public Config()
			: base(ResolveType.Transient)
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
