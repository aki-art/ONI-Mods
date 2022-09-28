﻿using CrittersDropBones.Effects;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class SuperHotSoupConfig : IEntityConfig
    {
        public const string ID = Mod.PREFIX + "SuperHotSoup";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = FEntityTemplates.CreateSoup(
                ID,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SUPERHOTSOUP.NAME,
                STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SUPERHOTSOUP.DESC,
                "cdb_superhotsoup_kanim");

            var foodInfo = Util.FoodInfoBuilder.StandardFood(ID)
                .KcalPerUnit(2400)
                .Quality(FOOD.FOOD_QUALITY_AMAZING)
                .Effect(CDBEffects.HOT_FOOD)
                .Build();

            ModDb.FoodEffects.Add(ID, OnConsumed);

            var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
            return gameObject;
        }

        public void OnConsumed(Worker worker)
        {
            var sicknessInstance = worker.GetSicknesses().Get(Db.Get().Sicknesses.ColdBrain);
            if (sicknessInstance != null)
            {
                Game.Instance.savedInfo.curedDisease = true;
                sicknessInstance.Cure();
            }
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
