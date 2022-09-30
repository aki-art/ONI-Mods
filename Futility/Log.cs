using System;
using System.Collections.Generic;
using System.Reflection;

namespace FUtility
{
    public class Log
    {
        public static string modName = typeof(Log).Assembly.GetName().Name.Replace("Merged", "");
        private static string prefix = $"[{modName}]: ";

        public static void SetName(string name)
        {
            prefix = $"[{name}]: ";
        }

        public static void PrintVersion()
        {
            Info($"Loaded version {GetVersion()}");
        }

        public static void PrintVersion(KMod.UserMod2 mod)
        {
            Info($"Loaded {mod.mod.title}, v{mod.mod.packagedModInfo.version}");
        }

        public static string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static void Info(params object[] arg)
        {
            Debug.Log(prefix + string.Join(", ", arg));
        }

        public static void Warning(params object[] arg)
        {
            Debug.LogWarning(prefix + string.Join(", ", arg));
        }

        public static void Assert(string name, object arg)
        {
            if(arg == null)
            {
                Warning($"Assert failed, {name} is null");
            }
        }

        public static void Debuglog(params object[] arg)
        {
#if DEBUG
                Debug.Log(prefix + " (debug) " + string.Join(", ", arg));
#endif
        }

        public static void Error(object arg)
        {
            Debug.LogError(prefix + arg.ToString());
        }

        public static void PrintInstructions(List<HarmonyLib.CodeInstruction> codes)
        {
            Debuglog("\n");
            for(int i = 0; i < codes.Count; i++)
            {
                Debuglog(i + ": " + codes[i]);
            }
        }
    }
}
