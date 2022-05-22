using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class StainedGlassTiles
    {
        internal static Material veryShiny;

        public static List<TileInfo> tileInfos = new List<TileInfo>()
        {
            new TileInfo(SimHashes.Algae),
            new TileInfo(SimHashes.Aluminum),
            new TileInfo(SimHashes.Bitumen),
            new TileInfo(SimHashes.BleachStone),
            new TileInfo("Blood").SpecColor(ModAssets.Colors.bloodRed).NotSolid(),
            new TileInfo(SimHashes.Ceramic),
            new TileInfo(SimHashes.Cobalt).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new TileInfo(SimHashes.Copper),
            new TileInfo(SimHashes.DepletedUranium).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY).SpecColor(ModAssets.Colors.uraniumGreen),
            new TileInfo(SimHashes.Diamond).SpecColor(ModAssets.Colors.W_H_I_T_E),
            new TileInfo(SimHashes.Dirt),
            new TileInfo(SimHashes.Gold).SpecColor(ModAssets.Colors.gold),
            new TileInfo(SimHashes.Granite),
            new TileInfo(SimHashes.Ice),
            new TileInfo(SimHashes.IgneousRock),
            new TileInfo(SimHashes.Iron),
            new TileInfo(SimHashes.Isoresin),
            new TileInfo(SimHashes.Lead),
            new TileInfo(SimHashes.Lime),
            new TileInfo(SimHashes.Niobium),
            new TileInfo(SimHashes.Magma).NotSolid(),
            new TileInfo(SimHashes.Mud).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new TileInfo(SimHashes.Obsidian),
            new TileInfo(SimHashes.Polypropylene),
            new TileInfo(SimHashes.Regolith).SpecColor(ModAssets.Colors.W_H_I_T_E),
            new TileInfo(SimHashes.Rust),
            new TileInfo(SimHashes.Salt),
            new TileInfo(SimHashes.SandStone),
            new TileInfo(SimHashes.SedimentaryRock),
            new TileInfo("Slag"),
            new TileInfo("SolidBrass"),
            new TileInfo("SolidSilver"),
            new TileInfo("SolidZinc").SpecColor(ModAssets.Colors.uraniumGreen),
            new TileInfo(SimHashes.SlimeMold),
            new TileInfo(SimHashes.Snow).SpecColor(ModAssets.Colors.W_H_I_T_E),
            new TileInfo(SimHashes.Steel),
            new TileInfo(SimHashes.Sucrose).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new TileInfo(SimHashes.Sulfur),
            new TileInfo(SimHashes.SuperInsulator).SpecColor(ModAssets.Colors.extraPink),
            new TileInfo(SimHashes.TempConductorSolid),
            new TileInfo(SimHashes.Tungsten),
            new TileInfo(SimHashes.Water).NotSolid()
        };

        // need fast lookups for build menu
        public static Dictionary<Tag, Tag> tileTagDict = new Dictionary<Tag, Tag>();

        public static EffectorValues decor;

        public static void RegisterAll()
        {
            tileInfos.RemoveAll(t => t.IsInvalid);
            foreach (TileInfo info in tileInfos)
            {
                RegisterTile(info);
            }
        }

        private static void RegisterTile(TileInfo info)
        {
            DefaultStainedGlassTileConfig config = new DefaultStainedGlassTileConfig
            {
                name = info.ElementTag.ToString()
            };

            BuildingConfigManager.Instance.RegisterBuilding(config);

            BuildingDef def = Assets.GetBuildingDef(config.ID);
            if (def)
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
            public bool solid = true;

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
                if (specColor.HasValue)
                {
                    if (specColor == ModAssets.Colors.W_H_I_T_E)
                    {
                        def.BlockTileMaterial = GetVeryShiny(def);
                    }
                    else
                    {
                        def.BlockTileMaterial = new Material(def.BlockTileMaterial);
                        def.BlockTileMaterial.SetColor("_ShineColour", specColor.Value);
                    }
                }

                def.ShowInBuildMenu = true;
                def.RequiredDlcIds = dlcIds;
                def.BuildingComplete.AddTag(ModAssets.Tags.noBackwall);
            }

            private Material GetVeryShiny(BuildingDef def)
            {
                if (veryShiny == null)
                {
                    veryShiny = new Material(def.BlockTileMaterial);
                    veryShiny.SetColor("_ShineColour", ModAssets.Colors.W_H_I_T_E);
                }

                return veryShiny;
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

            public TileInfo NotSolid()
            {
                solid = false;
                return this;
            }
        }
    }
}
