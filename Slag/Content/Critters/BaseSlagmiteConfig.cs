using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Slag.Content.Critters
{
    public class BaseSlagmiteConfig
    {
        public static GameObject CreateBaseStalagmite(string id, string traidID, string name, string desc, float mass, string anim_file, bool isBaby, string symbolOverridePrefix = null)
        {
            var height = isBaby ? 1 : 3;
            var navGrid = isBaby ? Consts.NAV_GRID.WALKER_BABY : Consts.NAV_GRID.WALKER_1X1;
            var anim = Assets.GetAnim(anim_file);

            var gameObject = EntityTemplates.CreatePlacedEntity(
                id,
                name,
                desc,
                mass,
                anim,
                "idle_loop",
                Grid.SceneLayer.Creatures,
                1,
                height,
                DECOR.BONUS.TIER0);

            EntityTemplates.ExtendEntityToBasicCreature(
                gameObject,
                FactionManager.FactionID.Pest,
                traidID,
                navGrid,
                NavType.Floor,
                16,
                0.5f,
                CrabShellConfig.ID,
                1,
                false,
                false,
                288.15f,
                343.15f,
                243.15f,
                373.15f);

            if (symbolOverridePrefix != null)
            {
                gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(anim, symbolOverridePrefix);
            }

            gameObject.AddOrGet<Trappable>();
            gameObject.AddOrGet<LoopingSounds>();

            gameObject.AddOrGetDef<CreatureFallMonitor.Def>();

            var growthMonitor = gameObject.AddOrGetDef<ShellGrowthMonitor.Def>();
            growthMonitor.levelCount = 10;

            var damageMonitor = gameObject.AddOrGetDef<ShellDamageMonitor.Def>();
            damageMonitor.dropMass = 50f;
            damageMonitor.itemDroppedOnShear = CrabShellConfig.ID;

            var kPrefabID = gameObject.GetComponent<KPrefabID>();
            kPrefabID.AddTag(GameTags.Creatures.Walker);

            var choreTable = new ChoreTable.Builder()
                .Add(new DeathStates.Def())
                .Add(new AnimInterruptStates.Def())
                //.Add(new GrowUpStates.Def())
                .Add(new TrappedStates.Def())
                //.Add(new IncubatingStates.Def())
                .Add(new BaggedStates.Def())
                .Add(new FallStates.Def())
                .Add(new MinedStates.Def())
                .Add(new StunnedStates.Def())
                .Add(new DebugGoToStates.Def())
                .Add(new FleeStates.Def())
                .PushInterruptGroup()
                //.Add(new CreatureSleepStates.Def())
                //.Add(new FixedCaptureStates.Def())
                //.Add(new RanchedStates.Def())
                //.Add(new LayEggStates.Def())
                //.Add(new EatStates.Def())
                //.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
                //.Add(new CallAdultStates.Def())
                .PopInterruptGroup()
                .Add(new IdleStates.Def());

            EntityTemplates.AddCreatureBrain(gameObject, choreTable, GameTags.Creatures.Species.CrabSpecies, symbolOverridePrefix);

            return gameObject;
        }

        public static List<Diet.Info> BasicDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
        {
            var diet_tags = new HashSet<Tag>();
            diet_tags.Add(SimHashes.ToxicSand.CreateTag());
            diet_tags.Add(RotPileConfig.ID.ToTag());

            return new List<Diet.Info>
            {
                new Diet.Info(
                    diet_tags,
                    poopTag,
                    caloriesPerKg,
                    producedConversionRate,
                    diseaseId,
                    diseasePerKgProduced,
                    false,
                    false)
            };
        }

        public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diets, float referenceCaloriesPerKg, float minPoopSizeInKg)
        {
            var diet = new Diet(diets.ToArray());

            var calorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
            calorieMonitor.diet = diet;
            calorieMonitor.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;

            var solidConsumer = prefab.AddOrGetDef<SolidConsumerMonitor.Def>();
            solidConsumer.diet = diet;

            return prefab;
        }
    }
}
