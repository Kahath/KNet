/*
 * Copyright © Kahath 2015
 * Licensed under MIT license.
 */

using ServerFramework.Database.Base.Context;
using ServerFramework.Database.Base.Entity;
using System;

namespace ServerFramework.Database.Base.Repository
{
	public abstract class RepositoryBase<T> : IDisposable where T : IEntity
	{
		#region Fields

		private DBContextBase _context;

		#endregion

		#region Properties

		protected DBContextBase Context
		{
			get { return _context; }
			set { _context = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates instance of <see cref="Base.RepositoryBase{T}"/> type.
		/// </summary>
		/// <param name="context">Instance of <see cref="Base.DBContextBase"/> type.</param>
		public RepositoryBase(DBContextBase context)
		{
			Context = context;
		}

		#endregion

		#region Methods

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes object
		/// </summary>
		/// <param name="disposing">disposing</param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				Context.Dispose();
			}
		}

		#endregion

		#endregion
	}
}
