/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using System.Net;

namespace ServerFramework.Configuration.Helpers
{
	public class SocketListenerSettings
	{
		#region Fields

		private int _maxConnections;
		private int _numberOfSaeaForRecSend;
		private int _backlog;
		private int _maxSimultaneousAcceptOps;
		private int _bufferSize;
		private int _headerLength;
		private IPEndPoint _localEndPoint;

		#endregion

		#region Properties

		public int MaxConnections
		{
			get { return _maxConnections; }
		}

		public int NumberOfSaeaForRecSend
		{
			get { return _numberOfSaeaForRecSend; }
		}

		public int Backlog
		{
			get { return _backlog; }
		}

		public int MaxAcceptOps
		{
			get { return _maxSimultaneousAcceptOps; }
		}

		public int HeaderLength
		{
			get { return _headerLength; }
		}

		public int BufferSize
		{
			get { return _bufferSize; }
		}

		public IPEndPoint LocalEndPoint
		{
			get { return _localEndPoint; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Configuration.SocketListenerSettings"/> type.
		/// </summary>
		/// <param name="maxConnections">Maximum number of connections allowed on server.</param>
		/// <param name="backlog">Number of queued connections if maximum number is surpassed.</param>
		/// <param name="maxAcceptOps">Maximum number of SocketAsyncEventArgs objects for accepting connections.</param>
		/// <param name="bufferSize">Buffer size for each SocketAsyncEventArgs object.</param>
		/// <param name="headerLength">Length of message header.</param>
		/// <param name="localEndPoint">IP address and port of listening.</param>
		internal SocketListenerSettings(int maxConnections, int backlog,
			int maxAcceptOps, int bufferSize,
			int headerLength, IPEndPoint localEndPoint)
		{
			_maxConnections = maxConnections;
			_numberOfSaeaForRecSend = maxConnections;
			_backlog = backlog;
			_maxSimultaneousAcceptOps = maxAcceptOps;
			_bufferSize = bufferSize;
			_headerLength = headerLength;
			_localEndPoint = localEndPoint;
		}

		#endregion
	}
}
