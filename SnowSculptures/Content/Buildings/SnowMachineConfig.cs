using Database;
using TUNING;
using UnityEngine;

namespace SnowSculptures.Content.Buildings
{
    internal class SnowMachineConfig : IBuildingConfig
    {
        public static string ID = "SnowSculptures_ShowMachine";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               1,
               "farmtile_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
               MATERIALS.RAW_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER0,
               BuildLocationRule.Anywhere,
               DECOR.NONE,
               NOISE_POLLUTION.NONE
           );

            def.AudioCategory = AUDIO.CATEGORY.GLASS;
            def.ViewMode = OverlayModes.Power.ID;

            def.RequiresPowerInput = true;
            def.ExhaustKilowattsWhenActive = Mod.Settings.SnowMachinePower.ExhaustKilowattsWhenActive;
            def.EnergyConsumptionWhenActive = Mod.Settings.SnowMachinePower.EnergyConsumptionWhenActive;
            def.SelfHeatKilowattsWhenActive = Mod.Settings.SnowMachinePower.SelfHeatKilowattsWhenActive;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            var snowMachine = go.AddComponent<SnowMachine>();
            snowMachine.speed = 1f;
            snowMachine.turbulence = 0.03f;
            snowMachine.lifeTime = 3f;
            snowMachine.density = 5f;

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGetDef<PoweredController.Def>();
        }
    }
}
