using Harmony;
using static SandboxToolParameterMenu.SelectorValue;

namespace WorldTraitsPlus
{
	// -------- FOR TESTING ONLY
	// -------- REMOVE BEFORE RELEASE

	class SandboxToolsTester
	{
		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			internal static void Postfix(SandboxToolParameterMenu __instance)
			{
				AddToSpawnerMenu(__instance);
			}
		}

		private static void AddToSpawnerMenu(SandboxToolParameterMenu instance)
		{
			var selector = instance.entitySelector;
			var filters = ListPool<SearchFilter, SandboxToolParameterMenu>.Allocate();
			filters.AddRange(selector.filters);

			filters.Add(new SearchFilter("Test",
				(entity) => {
					var prefab = entity as KPrefabID;
					bool ok = prefab != null;
					if (ok)
					{
						string name = prefab.PrefabTag.Name;
						//ok = name == WorldTraits.MegaMeteorConfig.ID || name == WorldTraits.IceCometConfig.ID;
						ok = name == WorldEvents.EarthQuakeConfig.ID ||
						name == WorldEvents.SinkHoleConfig.ID;
					}
					return ok;
				}, null, Def.GetUISprite(Assets.GetPrefab("MushBar"))));

			foreach (var filter in filters)
				if (filter.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL)
				{
					var oldCondition = filter.condition;
					filter.condition = (entity) => {
						var prefab = entity as KPrefabID;
						bool ok = prefab != null && prefab.GetComponent<Comet>() != null;//Tuning.meteors.Contains(prefab.PrefabTag.Name);
						return ok || oldCondition.Invoke(entity);
					};
				}

			// Add matching assets
			var options = ListPool<object, SandboxToolParameterMenu>.Allocate();
			foreach (var prefab in Assets.Prefabs)
				foreach (var filter in filters)
					if (filter.condition(prefab))
					{
						options.Add(prefab);
						break;
					}
#if DEBUG
			Debug.Log("added to menu-------------------------------");
#endif
			selector.options = options.ToArray();
			selector.filters = filters.ToArray();
			options.Recycle();
			filters.Recycle();
		}
	}
}

