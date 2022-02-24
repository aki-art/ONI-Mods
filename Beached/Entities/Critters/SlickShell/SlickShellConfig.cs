using Klei.AI;
using UnityEngine;
using static Beached.STRINGS.CREATURES.SPECIES;

namespace Beached.Entities.Critters.SlickShell
{
    internal class SlickShellConfig : IEntityConfig
    {
        public const string ID = "Beached_SlickShell";
        public const string EGG_ID = "Beached_SlickShell_Egg";
        public const string BASE_TRAIT_ID = "Beached_SlickShellTrait";

        public GameObject CreatePrefab()
        {
            var prefab = CreateBasePrefab();

            ExtendToWildCreature(prefab);
            ConfigureBaseTrait(BEACHED_SLICKSHELL.NAME);
            ExtendToFertileCreature(prefab);

            var moistureMonitor = prefab.AddOrGetDef<AI.MoistureMonitor.Def>();
            moistureMonitor.lubricant = Elements.Mucus;
            moistureMonitor.lubricantMassKg = 0.1f;
            moistureMonitor.lubricantTemperatureKelvin = 300;

            return prefab;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        private static GameObject CreateBasePrefab()
        {
            return BaseSnailConfig.CreatePrefab(ID, BEACHED_SLICKSHELL.NAME, BEACHED_SLICKSHELL.DESC, "pincher_kanim", BASE_TRAIT_ID);
        }

        private void ExtendToFertileCreature(GameObject prefab)
        {
            EntityTemplates.ExtendEntityToFertileCreature(
                prefab,
                EGG_ID,
                BEACHED_SLICKSHELL.EGG_NAME,
                BEACHED_SLICKSHELL.DESC,
                "egg_pincher_kanim",
                CrabTuning.EGG_MASS,
                BabySlickShellConfig.ID,
                60.000004f,
                20f,
                SlickShellTuning.EGG_CHANCES_BASE,
                CrabConfig.EGG_SORT_ORDER);
        }

        private void ExtendToWildCreature(GameObject prefab)
        {
            EntityTemplates.ExtendEntityToWildCreature(prefab, CrabTuning.PEN_SIZE_PER_CREATURE);
        }

        private void ConfigureBaseTrait(string name)
        {
            var trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
            trait.Add(new AttributeModifier(Amounts.Moisture.maxAttribute.Id, 100f, name));
        }

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
