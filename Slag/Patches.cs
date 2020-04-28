using Database;
using Harmony;
using ProcGen;
using Slag.Buildings;
using Slag.Critter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComplexRecipe;
using Utils;
using static Slag.SlagStrings.SLAGSTRINGS.BUILDINGS.PREFABS;

namespace Slag
{
    class Patches
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                GameTags.MaterialBuildingElements.Add(ModAssets.slagWoolTag);
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {

            public static void Prefix()
            {
                string ManualInsulatedDoorID = ManualInsulatedDoorConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.NAME", ManualInsulatedDoorConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.EFFECT", ManualInsulatedDoorConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ManualInsulatedDoorID}.DESC", ManualInsulatedDoorConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", ManualInsulatedDoorConfig.ID);


                string MechanizedInsulatedDoorID = InsulatedMechanizedAirlockConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.NAME", InsulatedMechanizedAirlockConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.EFFECT", InsulatedMechanizedAirlockConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MechanizedInsulatedDoorID}.DESC", InsulatedMechanizedAirlockConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", InsulatedMechanizedAirlockConfig.ID);

                string SpinnedID = SpinnerConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.NAME", SpinnerConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.EFFECT", SpinnerConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SpinnedID}.DESC", SpinnerConfig.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", SpinnerConfig.ID);

                string FiltrationTileConfigID = FiltrationTileConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FiltrationTileConfigID}.NAME", FILTRATION_TILE.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FiltrationTileConfigID}.EFFECT", FILTRATION_TILE.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FiltrationTileConfigID}.DESC", FILTRATION_TILE.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", FiltrationTileConfig.ID);

                string InsulatedStorageLockerID = InsulatedStorageLockerConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedStorageLockerID}.NAME", INSULATED_STORAGE_LOCKER.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedStorageLockerID}.EFFECT", INSULATED_STORAGE_LOCKER.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedStorageLockerID}.DESC", INSULATED_STORAGE_LOCKER.DESC);

                ModUtil.AddBuildingToPlanScreen("Base", InsulatedStorageLockerConfig.ID);
            }
        }
        


        [HarmonyPatch(typeof(EquipmentConfigManager))]
        [HarmonyPatch("RegisterEquipment")]
        public static class EquipmentConfigManager_RegisterEquipment_Patch
        {
            public static void Postfix()
            {
                var cloth = Assets.Prefabs[Assets.Prefabs.Count - 1];
                SpinnerConfig.AddRecipe(new RecipeElement(cloth.PrefabID(), 1f), new RecipeElement(BasicFabricConfig.ID, 1f), "Desc", 5);
            }
        }


        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var myRandom = new SeededRandom(0);
                var refinedMetalOptions = new List<WeightedMetalOption>()
                {
                    new WeightedMetalOption(SimHashes.Aluminum,             .8f),
                    new WeightedMetalOption(SimHashes.Copper,               .8f),
                    new WeightedMetalOption(SimHashes.Gold,                 .3f),
                    new WeightedMetalOption(SimHashes.Iron,                 1f),
                    new WeightedMetalOption(SimHashes.Lead,                 .5f),
                    new WeightedMetalOption(SimHashes.Mercury,              .6f),
                    new WeightedMetalOption(SimHashes.Niobium,              .02f),
                    new WeightedMetalOption(SimHashes.Steel,                .05f),
                    new WeightedMetalOption(SimHashes.TempConductorSolid,   .01f),
                    new WeightedMetalOption(SimHashes.Tungsten,             .03f),
                };

                List<string> results = new List<string>();
                for (var i = 0; i < 1000; i++)
                {
                    var chosenMetal = WeightedRandom.Choose(refinedMetalOptions, myRandom);
                    //Console.WriteLine(chosenMetal.element.ToString());
                    results.Add(chosenMetal.element.ToString());
                }

                var query = results
                    .GroupBy(s => s)
                    .Select(g => new { Name = g.Key, Count = g.Count() });

              
                foreach (var result in query)
                {
                    Console.WriteLine("Name: {0}, Count: {1}", result.Name, result.Count);
                }


                var techList = new List<string>(Techs.TECH_GROUPING["TemperatureModulation"]) {
                        ManualInsulatedDoorConfig.ID,
                        InsulatedMechanizedAirlockConfig.ID };

                Techs.TECH_GROUPING["TemperatureModulation"] = techList.ToArray();
            }
        }
    }
}
