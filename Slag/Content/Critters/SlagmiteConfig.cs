using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Slag.Content.Critters
{
    public class SlagmiteConfig : IEntityConfig
    {
        public const string ID = "Slag_Slagmite";
        public const string BASE_TRAIT_ID = "Slag_SlagmiteBaseTrait";
        public const string EGG_ID = "Slag_SlagmiteEgg";

        public GameObject CreatePrefab()
        {
            var prefab = BaseSlagmiteConfig.CreateBaseStalagmite(
                ID,
                BASE_TRAIT_ID,
                STRINGS.CREATURES.SPECIES.SLAGMITE.NAME,
                STRINGS.CREATURES.SPECIES.SLAGMITE.DESC,
                30f,
                "slagmite_kanim",
                false);

            EntityTemplates.ExtendEntityToWildCreature(prefab, CREATURES.SPACE_REQUIREMENTS.TIER3);

            prefab.AddOrGet<MineableCreature>().allowMining = true;

            var trait = Db.Get().CreateTrait(
                BASE_TRAIT_ID,
                STRINGS.CREATURES.SPECIES.SLAGMITE.NAME,
                STRINGS.CREATURES.SPECIES.SLAGMITE.NAME,
                null,
                false,
                null,
                true,
                true);

            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, STRINGS.CREATURES.SPECIES.SLAGMITE.NAME));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, STRINGS.CREATURES.SPECIES.SLAGMITE.NAME));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, STRINGS.CREATURES.SPECIES.SLAGMITE.NAME));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 45f, STRINGS.CREATURES.SPECIES.SLAGMITE.NAME));

            SetupDiet(prefab);

            return prefab;

        }

        private void SetupDiet(GameObject prefab)
        {
            var slags = new HashSet<Tag>
            {
                Elements.Slag.CreateTag()
            };

            var regoliths = new HashSet<Tag>
            {
               SimHashes.Regolith.CreateTag()
            };

            var diets = new List<Diet.Info>
            {
                new Diet.Info(
                    slags,
                    SimHashes.CrushedRock.CreateTag(),
                    SlagmiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
                    CREATURES.CONVERSION_EFFICIENCY.NORMAL),
                new Diet.Info(
                    regoliths,
                    SimHashes.CrushedRock.CreateTag(),
                    SlagmiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
                    CREATURES.CONVERSION_EFFICIENCY.BAD_1)
            };

            BaseSlagmiteConfig.SetupDiet(prefab, diets, SlagmiteTuning.BASE.CALORIES_PER_KG_OF_ORE, SlagmiteTuning.BASE.MIN_POOP_SIZE_IN_KG);
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
