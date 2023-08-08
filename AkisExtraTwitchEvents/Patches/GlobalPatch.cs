using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class GlobalPatch
	{
		[HarmonyPatch(typeof(Global), "Awake")]
		public class Global_Awake_Patch
		{
			public static void Postfix()
			{
				// duplicate entry, but the game expects both keys to exist
			}
		}
	}
}
