using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SandboxToolParameterMenu.SelectorValue;

namespace Entropea
{
    // testing only. remove before release
    class SandboxToolsTester
	{
		[HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			/// <summary>
			/// Applied after ConfigureEntitySelector runs.
			/// </summary>
			internal static void Postfix(SandboxToolParameterMenu __instance)
			{
				AddToSpawnerMenu(__instance);
			}
		}
		private static void AddToSpawnerMenu(SandboxToolParameterMenu instance)
		{
			// Transpiling it is possible (and a bit faster) but way more brittle
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
						ok = name == WorldTraits.MegaMeteorConfig.ID || name == WorldTraits.IceCometConfig.ID;
					}
					return ok;
				}, null, Def.GetUISprite(Assets.GetPrefab("MushBar"))));
		
			foreach (var filter in filters)
				if (filter.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL)
				{
					var oldCondition = filter.condition;
					filter.condition = (entity) => {
						var prefab = entity as KPrefabID;
						bool ok = prefab != null;
						if (ok)
						{
							string name = prefab.PrefabTag.Name;
							ok = name == GoldCometConfig.ID || name == CopperCometConfig.ID;
						}
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
