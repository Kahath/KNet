/*
 * Copyright (c) 2015. Kahath.
 * Licensed under MIT license.
 */

using ServerFramework.Configuration.Helpers;
using ServerFramework.Database.Context;
using ServerFramework.Database.Model.Application.PacketLog;
using ServerFramework.Enums;
using ServerFramework.Managers.Base;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServerFramework.Managers.Core
{
	public sealed class PacketLogManager : ManagerBase<PacketLogManager>, IDisposable
	{
		#region Fields

		private string _path;
		private List<PacketLogModel> _packetLog;
		private BlockingCollection<Packet> _packetLogQueue
			= new BlockingCollection<Packet>();

		#endregion

		#region Properties

		internal string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private BlockingCollection<Packet> PacketLogQueue
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
					{
						LogPacket(item);
					}
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
		private void LogPacket(Packet packet)
		{
			PacketLogType logtype = packet.Stream.Reader != null ? PacketLogType.CMSG : PacketLogType.SMSG;

			Client pClient = Manager.SessionMgr.GetClientBySession(packet.SessionId);
			
			PacketLogModel packetLog = new PacketLogModel();

			if (pClient != null)
			{
				packetLog.IP = pClient.IP;

				if (pClient.Token != null)
				{
					packetLog.ClientID = pClient.Token.ID;
				}
			}

			packetLog.Size = packet.Header.Length;
			packetLog.Opcode = packet.Header.Opcode;

			using (ApplicationContext context = new ApplicationContext())
			{
				packetLog.PacketLogTypeID = logtype == PacketLogType.CMSG ?
					context.PacketLogTypes.First(x => x.ID == (int)PacketLogType.CMSG && x.Active).ID :
					context.PacketLogTypes.First(x => x.ID == (int)PacketLogType.SMSG && x.Active).ID;
			}

			if (packet.Header.Length > 0)
			{
				packetLog.Message = logtype == PacketLogType.CMSG ?
					BitConverter.ToString(packet.Message) :
					BitConverter.ToString(packet.Message
					, packet.Header.Length > Int16.MaxValue ? ServerConfig.BigHeaderLength : ServerConfig.HeaderLength);
			}

			PacketLog.Add(packetLog);

			if (PacketLog.Count > ServerConfig.PacketLogSize)
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					context.PacketLogs.AddRange(PacketLog);
					context.SaveChanges();
				}

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
			PacketLogType logtype = packet.Stream.Reader != null ? PacketLogType.CMSG : PacketLogType.SMSG;
			
			if ((ServerConfig.PacketLogLevel & logtype) == logtype && packet.Header.IsForLog)
				PacketLogQueue.Add(packet);
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
