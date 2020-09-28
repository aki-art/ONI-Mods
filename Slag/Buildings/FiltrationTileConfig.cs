using System.Collections.Generic;
using TUNING;
using UnityEngine;
using FUtility;

namespace Slag.Buildings
{
    class FiltrationTileConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "FiltrationTile";
        private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1]
        {
            new CellOffset(1, 0)
        };

        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: "farmtilerotating_kanim",
                hitpoints: 100,
                construction_time: 30f,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
                construction_materials: MATERIALS.FARMABLE,
                melting_point: 1600f,
                build_location_rule: BuildLocationRule.Tile,
                decor: BUILDINGS.DECOR.NONE,
                noise: NOISE_POLLUTION.NONE);

            BuildingTemplates.CreateFoundationTileDef(def);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;
            def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
            def.AudioCategory = "HollowMetal";
            def.AudioSize = "small";
            def.BaseTimeUntilRepair = -1f;
            def.SceneLayer = Grid.SceneLayer.TileMain;
            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            def.PermittedRotations = PermittedRotations.FlipV;
            def.isSolidTile = true;
            def.DragBuild = true;

            return def;
        }
        private ElementConverter.OutputElement outputElement(SimHashes element, float kgPerSecond)
        {
            return new ElementConverter.OutputElement(
                    kgPerSecond: kgPerSecond,
                    element: element,
                    minOutputTemperature: 0f,
                    useEntityTemperature: true,
                    storeOutput: true,
                    outputElementOffsetx: 0,
                    outputElementOffsety: 0);
        }

        private ElementConverter.ConsumedElement[] inputElement(SimHashes element, float kgPerSecond)
        {
            return new ElementConverter.ConsumedElement[]
            {
                new ElementConverter.ConsumedElement(tag: element.CreateTag(), kgPerSecond: kgPerSecond)
            };
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            SimCellOccupier simcelloccupier = go.AddOrGet<SimCellOccupier>();
            simcelloccupier.doReplaceElement = true;
            simcelloccupier.notifyOnMelt = true;

            Storage storage = go.AddComponent<Storage>();
            storage.storageFilters = new List<Tag> { GameTags.Liquid };
            storage.capacityKg = 10f;

            ElementConsumer elementconsumer = go.AddOrGet<PassiveElementConsumer>();
            elementconsumer.configuration = ElementConsumer.Configuration.AllLiquid;
            elementconsumer.consumptionRate = 100f;
            elementconsumer.consumptionRadius = 1;
            elementconsumer.showInStatusPanel = true;
            elementconsumer.sampleCellOffset = new Vector3(0f, 1f, 0f);
            elementconsumer.isRequired = false;
            elementconsumer.storeOnConsume = true;
            elementconsumer.showDescriptor = false;
            elementconsumer.storage = storage;


            var elementConverter = go.AddComponent<ElementConverter>();
            elementConverter.consumedElements = inputElement(SimHashes.DirtyWater, 100f);
            elementConverter.outputElements = new ElementConverter.OutputElement[]
            {
                outputElement(SimHashes.Water, 70f),
                outputElement(SimHashes.ToxicSand, 30f)
            };

            var elementConverter2 = go.AddComponent<ElementConverter>();
            elementConverter2.consumedElements = inputElement(SimHashes.SaltWater, 100f);
            elementConverter2.outputElements = new ElementConverter.OutputElement[]
            {
                outputElement(SimHashes.Water, 70f),
                outputElement(SimHashes.Salt, 30f)
            };

            var elementConverter3 = go.AddComponent<ElementConverter>();
            elementConverter3.consumedElements = inputElement(SimHashes.Brine, 100f);
            elementConverter3.outputElements = new ElementConverter.OutputElement[]
            {
                outputElement(SimHashes.SaltWater, 50f),
                outputElement(SimHashes.Salt, 50f)
            };
            elementConverter.useGUILayout = false;
            elementConverter2.useGUILayout = false;
            elementConverter3.useGUILayout = false;

            /*				CopyBuildingSettings cbs = go.AddOrGet<CopyBuildingSettings>();
							cbs.copyGroupTag = GameTags.Farm;*/

            //go.AddComponent<FiltrationTileWorkable>();

            go.AddComponent<FiltrationTile5>();

            go.AddOrGet<AnimTileable>();
            Prioritizable.AddRef(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles, false);

            ElementConsumer consumer = go.GetComponent<ElementConsumer>();
            if (consumer != null)
                consumer.EnableConsumption(true);

        }

        public void OnSpawn(GameObject inst)
        {
            Debug.Log("OnSpawn");
            ElementConsumer consumer = inst.GetComponent<ElementConsumer>();
            if (consumer != null)
            {
                consumer.EnableConsumption(true);
            }
        }


    }
}
