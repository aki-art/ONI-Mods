using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class FullBodyUIMinionWidgetPatch
	{
		public static bool recentlyDyedRando = false;
		// dyes full body dupes to have their proper hair color. such as the skill screen dupes or those in the clothing station

		[HarmonyPatch(typeof(FullBodyUIMinionWidget), "UpdateClothingOverride", typeof(SymbolOverrideController), typeof(MinionIdentity), typeof(StoredMinionIdentity))]
		public class FullBodyUIMinionWidget_UpdateClothingOverride_Patch
		{
			public static void Postfix(FullBodyUIMinionWidget __instance, MinionIdentity identity)
			{
				if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 31))
					return;

				if (!Mod.Settings.UIDupePreviews)
					return;

				if (__instance.animController == null || !(__instance.animController is KBatchedAnimController) || !__instance.animController.enabled)
					return;

				if (identity == null || identity.personalityResourceId == HashedString.Invalid || identity.personalityResourceId == null)
					return;

				if (Db.Get().Personalities.TryGet(identity.personalityResourceId) == null)
					return;

				// possibly not compatible with future mods that also try to dye hair.
				// but that will be dealt with when it's neccessary.
				if (identity.TryGetComponent(out CustomDupe dye) && dye.dyedHair)
				{
					recentlyDyedRando = true;
					if (dye.hairColor != null)
						CustomDupe.TintHair(__instance.animController, dye.hairColor);
				}
				else
				{
					if (recentlyDyedRando)
						CustomDupe.TintHair(__instance.animController, Color.white);

					recentlyDyedRando = false;
				}
			}
		}
	}
}
