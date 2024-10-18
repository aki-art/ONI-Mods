using HarmonyLib;

namespace Moonlet.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				MoonletMods.Instance.moonletMods.Do(mod =>
				{
					Mod.spritesLoader.LoadSprites(__instance, mod.Value);
				});
			}

			[HarmonyPriority(Priority.HigherThanNormal)]
			[HarmonyPostfix]
			public static void EarlyPostfix(Assets __instance)
			{
				Mod.recipesLoader.ApplyToActiveTemplates(template => template.LoadContent());
			}
		}
	}
}