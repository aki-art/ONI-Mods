using System;

namespace Asphalt
{
    // Simple log helper, so logs by this mod are easily identifiable.
    class Log
    {
        private static readonly string prefix = $"[{typeof(Log).Assembly.GetName().Name}]: ";

        public static void Info(object arg)
        {
            try
            {
                Debug.Log(prefix + arg.ToString());
            }
            catch (Exception e)
            {
                Warn(e);
            }
        }

        public static void Warning(object arg)
        {
            try
            {
                Debug.LogWarning(prefix + arg.ToString());
            }
            catch (Exception e)
            {
                Warn(e);
            }
        }

        // only show up in debug versions
        public static void Debuglog(object arg)
        {
#if DEBUG
            try
            {
               Debug.Log(prefix + " (debug) " + arg.ToString());
            }
            catch (Exception e)
            {
                Warn(e);
            }
#endif
        }

        public static void Error(object arg)
        {
            try
            {
                Debug.LogError(prefix + arg.ToString());
            }
            catch (Exception e)
            {
                Warn(e);
            }
        }

        private static void Warn(Exception e)
        {
            Debug.LogWarning($"{prefix} Could not write to log: {e}");
        }
    }
}
