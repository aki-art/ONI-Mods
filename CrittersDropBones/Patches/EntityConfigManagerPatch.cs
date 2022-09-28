using CrittersDropBones.Buildings.SlowCooker;
using CrittersDropBones.Integration;
using CrittersDropBones.Integration.SpookyPumpkin;
using CrittersDropBones.Settings;
using HarmonyLib;
using System.Linq;

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
                    var creatureBrain = critter.GetComponent<CreatureBrain>();

                    if(creatureBrain == null)
                    {
                        continue;
                    }

                    var species = creatureBrain.species.ToString();

                    if (DropsConfig.bones.TryGetValue(species, out var drop))
                    {
                        if (critter.TryGetComponent(out Butcherable butcherable))
                        {
                            var isBaby = critter.GetDef<BabyMonitor.Def>() != null;
                            var amount = isBaby ? drop.amountBaby : drop.amountAdult;

                            if (amount > 0)
                            {
                                var newDrops = Enumerable.Repeat(drop.drop, amount);

                                if (butcherable.drops == null)
                                {
                                    butcherable.drops = newDrops.ToArray();
                                }
                                else
                                {
                                    butcherable.drops = butcherable.drops.AddRangeToArray(newDrops.ToArray());
                                }
                            }
                        }
                    }
                }
            }

            [HarmonyPostfix]
            [HarmonyPriority(Priority.LowerThanNormal)]
            public static void LatePostfix()
            {
                RecipeUtil.ConfigureRecipes(SlowCookerConfig.ID);
            }
        }
    }
}
