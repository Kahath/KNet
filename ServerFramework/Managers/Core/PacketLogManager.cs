/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.PacketLog;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ServerFramework.Managers.Core
{
	public sealed class PacketLogManager : ManagerBase<PacketLogManager>, IDisposable
	{
		#region Fields

		private string _path;
		private List<PacketLogModel> _packetLog;
		private BlockingCollection<PacketLogItem> _packetLogQueue
			= new BlockingCollection<PacketLogItem>();

		#endregion

		#region Properties

		internal string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private BlockingCollection<PacketLogItem> PacketLogQueue
		{
			get { return _packetLogQueue; }
			set { _packetLogQueue = value; }
		}

		private List<PacketLogModel> PacketLog
		{
			get
			{
				if (_packetLog == null)
					_packetLog = new List<PacketLogModel>();

				return _packetLog;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates new instance of <see cref="ServerFramework.Managers.Core.PacketLogManager"/> type.
		/// </summary>
		PacketLogManager()
		{
			Init();
		}

		#endregion

		#region Methods

		#region Init

		/// <summary>
		/// Initialises PacketLogManager.
		/// </summary>
		protected override void Init()
		{
			Thread logThread = new Thread(() =>
			{
				while (true)
				{
					var item = PacketLogQueue.Take();

					if (item != null)
						LogPacket(item);
				}
			});

			logThread.IsBackground = true;
			logThread.Start();
		}

		#endregion

		#region LogPacket

		/// <summary>
		/// Logs packet.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		private void LogPacket(PacketLogItem logItem)
		{
			PacketLogModel packetLog = new PacketLogModel();

			if (logItem.Client != null)
			{
				packetLog.IP = logItem.Client.IP;

				if (logItem.Client.Token != null)
					packetLog.ClientID = logItem.Client.Token.ID;
			}

			packetLog.Size = logItem.PacketHeader.Length;
			packetLog.Opcode = logItem.PacketHeader.Opcode;
			packetLog.PacketLogTypeID = (int)logItem.PacketLogType;

			if (logItem.PacketHeader.Length > 0)
			{
				packetLog.Message =	BitConverter.ToString(logItem.PacketMessage
					, logItem.PacketHeader.Length > Int16.MaxValue ? ServerConfig.BigHeaderLength : ServerConfig.HeaderLength);
			}

			PacketLog.Add(packetLog);

			if (PacketLog.Count > ServerConfig.PacketLogSize)
			{
				Manager.DatabaseMgr.AddOrUpdate<ApplicationContext, PacketLogModel>(true, PacketLog.ToArray());

				PacketLog.Clear();
			}
		}

		#endregion

		#region Log

		/// <summary>
		/// Adds packet to queue for logging.
		/// </summary>
		/// <param name="packet">Instance of <see cref="ServerFramework.Network.Packets.Packet"/> type.</param>
		internal void Log(Packet packet)
		{
			if ((ServerConfig.PacketLogLevel & packet.LogType) == packet.LogType && packet.Header.IsForLog)
			{
				byte[] message = packet.ToArray();
				Client pClient = Manager.SessionMgr.GetClientBySession(packet.SessionId);

				if (pClient != null)
				{
					PacketLogItem packetLog = new PacketLogItem(pClient, packet.Header, message, packet.LogType);
					PacketLogQueue.Add(packetLog);
				}
			}
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes object.
		/// </summary>
		public void Dispose()
		{
			_packetLogQueue.Dispose();
		}

		#endregion

		#endregion
	}
}
