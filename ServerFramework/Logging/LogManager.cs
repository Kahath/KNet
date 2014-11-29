using ServerFramework.Configuration;
using ServerFramework.Constants.Misc;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Logging
{
    public static class LogManager
    {
        #region Methods

        #region SetLoggerAsync

        private static async Task SetLoggerAsync(LogType type, string message, params object[] args)
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
                Console.ForegroundColor = color;
                Console.WriteLine(string.Format(
                    "[{0}] {1}", DateTime.Now.ToLongTimeString(), string.Format(message, args)));
            }

            await Task.Delay(1);
        }

        #region Log

        public static async void Log(LogType type, string message, params object[] args)
        {
            await SetLoggerAsync(type, message, args);
        }

        #endregion

        #endregion

        #endregion     
    }
}
