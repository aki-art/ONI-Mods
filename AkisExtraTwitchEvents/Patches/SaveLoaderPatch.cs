using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), typeof(IReader))]
		public class SaveLoader_Load_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				AkisTwitchEvents.OnWorldLoaded();
			}
		}
	}
}
