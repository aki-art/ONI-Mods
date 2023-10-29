/*using HarmonyLib;
using System;

namespace Twitchery.Patches
{
	// TODO 
	internal class TEMP
	{
		[Obsolete("Don't forget to remove this")]
		public static void Patch(Harmony harmony)
		{
			var type = Type.GetType("ONITwitch.Content.Cmps.PocketDimension.PocketDimension, ONITwitch");
			var method = AccessTools.Method(type, "UpdateImageTimer");
			var prefix = AccessTools.Method(typeof(TEMP), nameof(Prefix));

			harmony.Patch(method, new HarmonyMethod(prefix));
		}

		private static bool Prefix() => false;
	}
}
*/