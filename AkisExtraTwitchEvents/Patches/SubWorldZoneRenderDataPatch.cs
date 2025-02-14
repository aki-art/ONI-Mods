using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	internal class SubWorldZoneRenderDataPatch
	{
		[HarmonyPatch(typeof(SubworldZoneRenderData), nameof(SubworldZoneRenderData.OnActiveWorldChanged))]
		public class SubworldZoneRenderData_OnActiveWorldChanged_Patch
		{
			public static void Postfix()
			{
				AkisTwitchEvents.Instance.RegenerateBackwallTexture();
			}
		}
	}
}
