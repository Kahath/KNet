using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace ServerFramework.Logging
{
    public class Log
    {

        #region Fields

        static BlockingCollection<Tuple<ConsoleColor, string>> logQueue =
            new BlockingCollection<Tuple<ConsoleColor, string>>();

        #endregion

        #region Methods

        #region Init

        /// <summary>
        /// Initialises Console logging
        /// </summary>
        public static void Init()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Initialising logger");
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var log = logQueue.Take();
                        if (log != null)
                        {
                            var msg = log.Item2;

                            Console.ForegroundColor = log.Item1;
                            Console.WriteLine(msg);
                        }
                    }
                    catch (Exception) { }
                }
            }).Start();
        }

        #endregion

        #region Message

        /// <summary>
        /// Adds message to queue for console logging
        /// </summary>
        /// <param name="type">Log type</param>
        /// <param name="message">message to write on console</param>
        /// <param name="args">message arguments</param>
        public static void Message(LogType type, string message, params object[] args)
        {
            SetLogger(type, message, args);
        }

        public static void Message(string message, params object[] args)
        {
            SetLogger(LogType.None, message, args);
        }

        #endregion

        #region SetLogger

        private static void SetLogger(LogType type, string message, params object[] args)
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
                logQueue.Add(Tuple.Create(color, string.Format(
                    "[{0}] {1}", DateTime.Now.ToLongTimeString(), string.Format(message, args))));
        }

        #endregion

        #endregion     
    }
}
