using HarmonyLib;
using Moonlet.Templates.EntityTemplates;

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
				Mod.recipesLoader.ApplyToActiveLoaders(template => template.LoadContent());
				Mod.traitsLoader.ApplyToActiveLoaders(template => template.LoadIcons());

				Log.Debug("assets patch");

				Mod.ApplyToAllActiveEntities(loader =>
				{
					Log.Debug("applying to " + loader.id);
					var template = loader.GetTemplate();

					if (template is EntityTemplate entityTemplate)
					{
						Log.Debug("entityTemplate");
						var anim = entityTemplate.Animation?.GetFile();

						if (anim == null)
							return;

						if (Mod.kanimExtensionsLoader.HasLoopingSounds((HashedString)anim))
						{
							Log.Debug("HasLoopingSounds");
							var prefab = Assets.TryGetPrefab(loader.id);
							if (prefab != null)
								prefab.AddOrGet<LoopingSounds>();
						}
					}
				}, true);
			}
		}
	}
}