using Harmony;
using System.Collections.Generic;
using UnityEngine;
using static ComplexRecipe;
using static STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER;

namespace RockGrinder
{
    public class CritterPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                foreach (GameObject critter in Assets.GetPrefabsWithComponent<Butcherable>())
                {
                    List<RecipeElement> result = new List<RecipeElement>();
                    foreach (var drop in critter.GetComponent<Butcherable>().Drops)
                    {
                        var d = Assets.TryGetPrefab(drop);
                        if (d != null)
                            result.Add(new RecipeElement(drop, d.GetComponent<PrimaryElement>().MassPerUnit));
                    }

                    float mass = critter.GetComponent<PrimaryElement>().MassPerUnit;
                    result.Add(new RecipeElement(SimHashes.Lime.CreateTag(), mass / 100f));
                    RecipeUtil.CreateRecipe(RockGrinderConfig.ID, critter.GetComponent<KPrefabID>().PrefabTag, 1, result.ToArray(), RECIPE_DESCRIPTION);
                }
            }
        }
    }
}
