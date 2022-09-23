using CrittersDropBones.Integration;
using CrittersDropBones.Integration.SpookyPumpkin;
using FUtility;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public static class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Postfix()
            {
                if(Mod.IsSpookyPumpkinHere)
                {
                    Helper.RegisterEntity(new PumpkinSoupConfig().CreatePrefab());
                }

                foreach (var critter in Assets.GetPrefabsWithComponent<Butcherable>())
                {
                    Log.Assert("Bones", Mod.Settings.Bones);
                    if (Mod.Settings.Bones is null)
                    {
                        return;
                    }

                    var ID = critter.PrefabID().ToString();

                    if (Mod.Settings.Bones.TryGetValue(ID, out var config))
                    {
                        if (critter.TryGetComponent(out Butcherable butcherable))
                        {
                            Log.Debuglog($"Adding drop to {critter.PrefabID()}");
                            foreach (var drop in config.Drops)
                            {
                                Log.Debuglog(drop.Drop, drop.Amount);

                                for (var i = 0; i < drop.Amount; i++)
                                {
                                    //butcherable.Drops = butcherable.Drops.AddToArray(drop.Drop);
                                    butcherable.drops = butcherable.drops.AddToArray(drop.Drop);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
