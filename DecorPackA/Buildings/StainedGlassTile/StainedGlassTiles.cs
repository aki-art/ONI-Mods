using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class StainedGlassTiles
    {
        public static List<TileInfo> tileInfos = new List<TileInfo>()
        {
            new TileInfo(SimHashes.Algae),
            new TileInfo(SimHashes.Aluminum),
            new TileInfo(SimHashes.Bitumen),
            new TileInfo("Blood").SpecColor(Color.red),
            new TileInfo(SimHashes.Cobalt).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new TileInfo(SimHashes.Copper),
            new TileInfo(SimHashes.DepletedUranium).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY).SpecColor(ModAssets.Colors.uraniumGreen),
            new TileInfo(SimHashes.Gold).SpecColor(ModAssets.Colors.gold),
            new TileInfo(SimHashes.Granite),
            new TileInfo(SimHashes.Ice).SpecColor(ModAssets.Colors.W_H_I_T_E),
            new TileInfo(SimHashes.IgneousRock),
            new TileInfo(SimHashes.Iron),
            new TileInfo(SimHashes.Lead),
            new TileInfo(SimHashes.Niobium),
            new TileInfo(SimHashes.Regolith).SpecColor(ModAssets.Colors.W_H_I_T_E),
            new TileInfo(SimHashes.Rust),
            new TileInfo(SimHashes.Salt),
            new TileInfo(SimHashes.SandStone),
            new TileInfo(SimHashes.SedimentaryRock),
            new TileInfo(SimHashes.SlimeMold),
            new TileInfo(SimHashes.Steel),
            new TileInfo(SimHashes.Sucrose).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new TileInfo(SimHashes.Sulfur),
            new TileInfo(SimHashes.SuperInsulator).SpecColor(ModAssets.Colors.extraPink),
            new TileInfo(SimHashes.TempConductorSolid),
            new TileInfo(SimHashes.Tungsten)
        };

        // need fast lookips for build menu
        public static Dictionary<Tag, Tag> tileTagDict = new Dictionary<Tag, Tag>();

        public static void RegisterAll()
        {
            foreach (TileInfo info in tileInfos)
            {
                RegisterTile(info);
            }
        }

        private static void RegisterTile(TileInfo info)
        {
            if (info.IsInvalid) return;

            DefaultStainedGlassTileConfig config = new DefaultStainedGlassTileConfig
            {
                name = info.ElementTag.ToString()
            };

            BuildingConfigManager.Instance.RegisterBuilding(config);

            BuildingDef def = Assets.GetBuildingDef(config.ID);
            if(def)
            {
                info.ConfigureDef(def);
                tileTagDict.Add(info.ElementTag, def.Tag);
                Log.Debuglog("Registered tile: ", info.ID);
            }
        }

        public class TileInfo
        {
            public string ID { get; private set; }

            public Tag ElementTag { get; private set; } = Tag.Invalid;

            private Color? specColor;
            private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

            public bool IsInvalid => ElementLoader.elements is null || ElementLoader.GetElement(ElementTag) is null;

            public TileInfo(Tag elementTag)
            {
                ElementTag = elementTag;
                SetID(elementTag);
            }

            public TileInfo(SimHashes elementHash) : this(elementHash.CreateTag()) { }

            private void SetID(Tag elementName) => ID = Mod.PREFIX + elementName.ToString() + "StainedGlassTile";

            public void ConfigureDef(BuildingDef def)
            {
                if(specColor.HasValue)
                {
                    def.BlockTileMaterial = new Material(def.BlockTileMaterial);
                    def.BlockTileMaterial.SetColor("_ShineColour", specColor.Value);
                }

                def.RequiredDlcIds = dlcIds;
            }

            public TileInfo DLC(string[] dlcIds)
            {
                this.dlcIds = dlcIds;
                return this;
            }

            public TileInfo SpecColor(Color specColor)
            {
                this.specColor = specColor;
                return this;
            }
        }
    }
}
