using System;

namespace FUtility
{
    public class Log
    {
        public static string modName = typeof(Log).Assembly.GetName().Name.Replace("Merged", "");
        private static string prefix = $"[{modName}]: ";

        public static void PrintVersion()
        {
            Info($"Loaded version {GetVersion()}");
        }

        public static string GetVersion() => typeof(Log).Assembly.GetName().Version.ToString();
        public static void Info(params object[] arg)
        {
            try
            {
                Debug.Log(prefix + string.Join(", ", arg));
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}
