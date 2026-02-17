using HarmonyLib;
using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class GeyserPatch
	{
		[HarmonyPatch(typeof(Geyser), "OnSpawn")]
		public class Geyser_OnSpawn_Patch
		{
			public static void Postfix(Geyser __instance)
			{
				if (__instance == null || __instance.gameObject == null)
					Log.Warning("wtf");

				if (AkisTwitchEvents.MaxDanger < Danger.Deadly)
				{
					__instance.gameObject.AddOrGet<Demolishable>();
				}
			}
		}
	}
}
