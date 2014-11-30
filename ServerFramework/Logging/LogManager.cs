using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
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

namespace ServerFramework.Logging
{
    public static class LogManager
    {
        #region Fields

        private static BlockingCollection<Tuple<ConsoleColor, string>> _consoleLogQueue
            = new BlockingCollection<Tuple<ConsoleColor, string>>();

        #endregion

        #region Properties

        internal static BlockingCollection<Tuple<ConsoleColor, string>> ConsoleLogQueue
        {
            get { return _consoleLogQueue; }
            set { _consoleLogQueue = value; }
        }

        #endregion

        #region Methods

        #region Init

        internal static void Init()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Thread logThread = new Thread(() =>
            {
                while (true)
                {
                    var item = ConsoleLogQueue.Take();

                    if (item != null)
                    {
                        try
                        {
                            Console.ForegroundColor = item.Item1;
                            Console.WriteLine(item.Item2);
                        }
                        catch (NullReferenceException) { }
                    }
                }

            });

            logThread.IsBackground = true;
            logThread.Start();
        }

        #endregion

        #region Message

        private static void Message(LogType type, string message, params object[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            ConsoleColor color;
            switch (type)
            {
                case LogType.Normal:
                    color = ConsoleColor.Green;
                    message = message.Insert(0, "System: ");
                    break;
                case LogType.Error:
                    color = ConsoleColor.Red;
                    message = message.Insert(0, "Error: ");
                    break;
                case LogType.Init:
                    color = ConsoleColor.Cyan;
                    break;
                case LogType.Database:
                    color = ConsoleColor.Yellow;
                    break;
                case LogType.Debug:
                    message = message.Insert(0, "Debug: ");
                    color = ConsoleColor.DarkRed;
                    break;
                case LogType.Dump:
                    color = ConsoleColor.DarkMagenta;
                    break;
                case LogType.Cmd:
                    color = ConsoleColor.Gray;
                    break;
                case LogType.Command:
                    color = ConsoleColor.Blue;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }

            if ((ServerConfig.LogLevel & type) == type)
            {
                string msg = string.Format(
                    "[{0}] {1}", DateTime.Now.ToLongTimeString(), string.Format(message, args));

                ConsoleLogQueue.Add(Tuple.Create<ConsoleColor, string>(color, msg));
            }

        }



        #endregion

        #region Log

        public static void Log(LogType type, string message, params object[] args)
        {
            Message(type, message, args);
        }

        #endregion

        #endregion     
    }
}
