using FUtility;
using Harmony;
using Klei;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Artable;

namespace DecorExpansion
{
    public class ArtablePatch
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                foreach(var prefab in Assets.GetPrefabsWithComponent<Artable>())
                {
                    prefab.GetComponent<KPrefabID>().prefabSpawnFn += go => SetVanillaVariants(prefab);
                }

                foreach (var item in CollectOverridesFromYAML())
                {
                    var prefab = Assets.TryGetPrefab(item.BuildingID);
                    if (prefab == null)
                    {
                        Log.Warning($"Tried to add an override to {item.BuildingID}, but that prefab doesn't exist");
                        continue;
                    }
                    prefab.GetComponent<KPrefabID>().prefabSpawnFn += go => SetOverrideStages(go, item);
                }
            }

            private static void SetVanillaVariants(GameObject prefab)
            {
                SymbolOverrideControllerUtil.AddToPrefab(prefab);
                var artOverrides = prefab.AddComponent<ArtableOverride>();
                artOverrides.defaultAnim = prefab.GetComponent<BuildingComplete>().Def.DefaultAnimState;
                artOverrides.targetSymbol = new KAnimHashedString("sculpt_temp");

                artOverrides.AddSymbolsFromArtable(prefab.GetComponent<Artable>());
            }

            public class OverrideEntryCollection
            {
                public OverrideEntry[] artableOverrides { get; set; }
            }

            public static List<OverrideEntry> CollectOverridesFromYAML()
            {
                List<OverrideEntry> elementEntryList = new List<OverrideEntry>();
                ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
                FileSystem.GetFiles(ModAssets.ModPath, "*.yaml", pooledList);

                foreach (FileHandle fileHandle in pooledList)
                {
                    OverrideEntryCollection elementEntryCollection = YamlIO.LoadFile<OverrideEntryCollection>(fileHandle.full_path);
                    if (elementEntryCollection != null)
                        elementEntryList.AddRange(elementEntryCollection.artableOverrides);
                }

                pooledList.Recycle();
                return elementEntryList;
            }

            private static void SetOverrideStages(GameObject go, OverrideEntry overrideInfo)
            {
                string animFile = overrideInfo.Animation + "_kanim";

                var artOverrides = go.GetComponent<ArtableOverride>();
                artOverrides.AddSymbols(Status.Ugly, overrideInfo.Ugly, animFile);
                artOverrides.AddSymbols(Status.Okay, overrideInfo.Average, animFile);
                artOverrides.AddSymbols(Status.Great, overrideInfo.Good, animFile);
            }
        }

        [HarmonyPatch(typeof(Artable), "SetStage")]
        public static class Artable_SetStage_Patch
        {
            public static void Prefix(ref string stage_id, List<Stage> ___stages)
            {
                // prevents a crash from disabled artable mods that didn't clean their extra Stages.
                string id = stage_id;
                if(!___stages.Any(s => s.id == id))
                {
                    stage_id = "Default";
                }
            }

            public static void Postfix(string stage_id, Artable __instance)
            {
                Status status = __instance.stages.FirstOrDefault(s => s.id == stage_id).statusItem;
                __instance.gameObject.GetComponent<ArtableOverride>().SetRandomStage(__instance.CurrentStatus);
            }
        }
    }
}
