using HarmonyLib;
using System;

namespace Moonlet.Patches
{
	public class KCrashReporterPatch
	{
		[HarmonyPatch(typeof(KCrashReporter), "ReportError")]
		public class KCrashReporter_ReportError_Patch
		{
			public static bool Prefix(string msg, string stack_trace)
			{
				try
				{
					var debug = UnityEngine.Debug.isDebugBuild;
					return true;
				}
				catch (Exception ex)
				{
					Log.Error($"Cannot display KCrash message. {ex.Message} {ex.StackTrace}");
					Log.Error($"Original error: {msg} {stack_trace}");
					return false;
				}
			}
		}
	}
}
