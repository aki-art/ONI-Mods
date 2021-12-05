using FUtility;
using HarmonyLib;
using UnityEngine;
using static CrittersDropBones.Settings.Config;

namespace CrittersDropBones.Patches
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.LoadGeneratedEntities))]
        public static class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Postfix()
            {
                foreach (GameObject critter in Assets.GetPrefabsWithComponent<Butcherable>())
                {
                    Log.Assert("Bones", Mod.Settings.Bones);
                    if (Mod.Settings.Bones is null) return;

                    string ID = critter.PrefabID().ToString();

                    if (Mod.Settings.Bones.TryGetValue(ID, out BoneDrops config))
                    {
                        if (critter.TryGetComponent(out Butcherable butcherable))
                        {
                            Log.Debuglog($"Adding drop to {critter.PrefabID()}");
                            foreach (BoneDropConfig drop in config.Drops)
                            {
                                Log.Debuglog(drop.Drop, drop.Amount);

                                for (int i = 0; i < drop.Amount; i++)
                                {
                                    butcherable.Drops = butcherable.Drops.AddToArray(drop.Drop);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
