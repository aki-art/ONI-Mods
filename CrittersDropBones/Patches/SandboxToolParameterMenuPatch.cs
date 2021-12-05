using HarmonyLib;
using System;
using System.Collections.Generic;
using static SandboxToolParameterMenu.SelectorValue;

namespace CrittersDropBones.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            internal static void Postfix(SandboxToolParameterMenu __instance)
            {
                List<SearchFilter> filters = new List<SearchFilter>(__instance.entitySelector.filters);

                foreach (SearchFilter filter in filters)
                {
                    if (filter.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL)
                    {
                        Func<object, bool> oldCondition = filter.condition;
                        filter.condition = entity => oldCondition.Invoke(entity) || entity is KPrefabID entityID && entityID.HasTag(ModAssets.Tags.Bone);
                        break;
                    }
                }

                __instance.entitySelector.filters = filters.ToArray();
                UpdateOptions(__instance, filters);
            }

            private static bool IsInSet(object entity, HashSet<Tag> set)
            {
                KPrefabID prefab = entity as KPrefabID;
                return prefab != null && set.Contains(prefab.PrefabID());
            }

            private static void UpdateOptions(SandboxToolParameterMenu __instance, List<SearchFilter> filters)
            {
                ListPool<object, SandboxToolParameterMenu>.PooledList options = ListPool<object, SandboxToolParameterMenu>.Allocate();
                foreach (KPrefabID prefab in Assets.Prefabs)
                {
                    foreach (SearchFilter filter in filters)
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