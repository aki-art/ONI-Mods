using Moonlet.Console;
using Moonlet.Scripts.UI;
using System.Collections.Generic;

namespace Moonlet
{
	public class Log
	{
		private static readonly HashSet<string> debugEnabledMods = new();

		private static bool genericDebugLogging = true;

		public static void EnableDebugLogging(string mod) => debugEnabledMods.Add(mod);

		public static void Info(object message, string mod = null) => Log_Internal(message, mod);

		public static void Warn(object message, string mod = null) => Log_Internal(message, "[WARNING] ", mod);

		public static void Error(object message, string mod = null) => Log_Internal(message, "[ERROR] ", mod);

		public static void Debug(object message, string mod = null)
		{
			if ((mod == null && genericDebugLogging) || debugEnabledMods.Contains(mod))
				Log_Internal(message, "[DEBUG] ", mod);
		}

		private static void Log_Internal(object message, string modifier = null, string mod = null)
		{
			if (!mod.IsNullOrWhiteSpace())
				mod = $"/{mod}";

			var msg = $"[Moonlet{mod}] {modifier}{message}";
			global::Debug.Log(msg);

			DevConsole.Log(msg);

			DevConsoleScreen.AddLogStr(msg);
		}
	}
}
