using CrittersDropBones.Integration;
using CrittersDropBones.Integration.SpookyPumpkin;
using CrittersDropBones.Settings;
using HarmonyLib;
using UnityEngine;

namespace CrittersDropBones.Patches
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public static class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Postfix()
            {
                if (Mod.IsSpookyPumpkinHere)
                {
                    Helper.RegisterEntity(new PumpkinSoupConfig().CreatePrefab());
                }

                foreach (var critter in Assets.GetPrefabsWithComponent<Butcherable>())
                {
                    if (critter.TryGetComponent(out CreatureBrain creatureBrain))
                    {
                        if (DropsConfig.bones.TryGetValue(creatureBrain.species.ToString(), out var drop))
                        {
                            AddExtraDrops(critter, drop);
                        }
                    }
                }
            }

            private static void AddExtraDrops(GameObject critter, DropsConfig.BoneDropConfig drop)
            {
                if (critter.TryGetComponent(out Butcherable butcherable))
                {
                    var isBaby = critter.GetDef<BabyMonitor.Def>() != null;
                    var amount = isBaby ? drop.amountBaby : drop.amountAdult;

                    if (amount > 0)
                    {
                        AddToArray(ref butcherable.drops, drop.drop, amount);
                    }
                }
            }

            private static void AddToArray<T>(ref T[] array, T element, int count)
            {
                array ??= new T[count];

                for (int i = 0; i < count; i++)
                {
                    array = array.AddToArray(element);
                }
            }
        }
    }
}
