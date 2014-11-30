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

using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using ServerFramework.Database;
using ServerFramework.Logging;
using ServerFramework.Managers;
using ServerFramework.Network.Packets;
using ServerFramework.Network.Socket;
using System;
using System.Net;
using System.Net.Sockets;

namespace ServerFramework
{
    #region Delegates

    public delegate bool CommandScriptHandler(params string[] args);
    public delegate void PacketHandler(UserToken token);
    public delegate void ManagerInitialisationEventHandler(object sender, EventArgs e);
    public delegate void PacketSendEventHandler(object sender, EventArgs e);
    public delegate void ServerEventHandler(object sender, SocketAsyncEventArgs e);
    public delegate void PacketManagerInvokeEventHandler(object sender, EventArgs e);

    #endregion

    public sealed class KahathFramework
    {
        #region Fields

        private SocketListenerSettings _socketSettings;

        #endregion

        #region Constructors

        public KahathFramework()
        {
            ServerConfig.Init();

            _socketSettings = new SocketListenerSettings(
                ServerConfig.MaxConnections, ServerConfig.Backlog
                , ServerConfig.MaxSimultaneousAcceptOps, ServerConfig.BufferSize
                , ServerConfig.HeaderLength, new IPEndPoint
                    (IPAddress.Parse(ServerConfig.BindIP), ServerConfig.BindPort));

            LogManager.Init();

            LogManager.Log(LogType.Init, "Initialising application database connection.");
            DB.Application.Init(ServerConfig.DBHost, ServerConfig.DBUser, ServerConfig.DBPass
                , ServerConfig.DBPort, ServerConfig.DBName);

            LogManager.Log(LogType.Init, "Initialising managers.");
            Manager.Init();

            LogManager.Log(LogType.Init, "Initialising server!");
            Server.GetInstance(_socketSettings);

            while(true)
            {
                Manager.CommandMgr.InvokeCommand(Console.ReadLine().ToLower());
            }
        }

        #endregion 
    }
}
