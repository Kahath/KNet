using ServerFramework.Constants.Misc;
using ServerFramework.Logging;
using System;
using System.Globalization;
using System.Reflection;

namespace ServerFramework.Singleton
{
    public class SingletonBase<T> where T : class
    {
        #region Fields

        private static volatile T _instance;
        private static object _syncObject = new object();

        #endregion

        #region Properties

        public static T GetInstance(params object[] args)
        {
            object ctor = null;

            if (_instance == null)
            {
                lock (_syncObject)
                {
                    if (_instance == null)
                    {
                        try
                        {
                            ctor = Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic
                            , null, args, CultureInfo.CurrentCulture);
                        }
                        catch (Exception)
                        {
                            Log.Message(LogType.Error, "Error with creating instance of {0} type"
                                , typeof(T));
                            Console.ReadLine();
                            Environment.Exit(0);
                        }

                        _instance = ctor as T;
                        return _instance;
                    }
                }
            }
            return _instance;
        }

        #endregion       
    }
}
