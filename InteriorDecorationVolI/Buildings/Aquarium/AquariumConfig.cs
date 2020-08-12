/*using FUtility;
using TUNING;
using UnityEngine;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class AquariumConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = ModAssets.Prefix + "Aquarium";
        public MBInfo Info => new MBInfo(ID, "Furniture", "Luxury");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 2,
               height: 2,
               anim: "sculpture_glass_kanim",
               hitpoints: 100,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.TRANSPARENTS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = true;
            def.Overheatable = true;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "slab";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag _)
        {
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);

            Storage storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.BAGABLE_CREATURES;
            storage.workAnims = new HashedString[]  {
                "place",
                "release"
            };
            storage.overrideAnims = new KAnimFile[] {
                Assets.GetAnim("anim_restrain_creature_kanim")
            };
            storage.workAnimPlayMode = KAnim.PlayMode.Once;
            storage.synchronizeAnims = false;
            storage.useGunForDelivery = false;
            storage.allowSettingOnlyFetchMarkedItems = false;

            go.AddComponent<FishTank>();
        }

        public override void DoPostConfigureComplete(GameObject _) { }
    }
}
*/