using Harmony;
using System;
using System.Collections.Generic;
using static SandboxToolParameterMenu.SelectorValue;

namespace WorldCreep.SandBox
{
    class EntitySpawnerPatches
    {
        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            internal static void Postfix(SandboxToolParameterMenu __instance)
            {
                List<SearchFilter> filters = new List<SearchFilter>(__instance.entitySelector.filters)
                {
                    new SearchFilter(
                    "World Events",
                    entity => IsInSet(entity, Tuning.worldEventIDs),
                    null,
                    Def.GetUISprite(Assets.GetPrefab(WorldEvents.EarthQuakeConfig.ID)))
                };

                foreach (var filter in filters)
                {
                    if (filter.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL)
                    {
                        var oldCondition = filter.condition;
                        filter.condition = entity => oldCondition.Invoke(entity) || IsInSet(entity, Tuning.meteorIDs);
                    }
                }

                __instance.entitySelector.filters = filters.ToArray();
                UpdateOptions(__instance, filters);
            }

            private static bool IsInSet(object entity, HashSet<Tag> set)
            {
                var prefab = entity as KPrefabID;
                return prefab != null && set.Contains(prefab.PrefabID());
            }

            private static void UpdateOptions(SandboxToolParameterMenu __instance, List<SearchFilter> filters)
            {
                var options = ListPool<object, SandboxToolParameterMenu>.Allocate();
                foreach (var prefab in Assets.Prefabs)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.condition(prefab))
                        {
                            options.Add(prefab);
                            break;
                        }
                    }
                }

                __instance.entitySelector.options = options.ToArray();
                options.Recycle();
            }
        }
    }
}
