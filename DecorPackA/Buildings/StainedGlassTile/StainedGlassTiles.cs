using FUtility;
using HarmonyLib;
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
            // new TileInfo("Blood").SpecColor(Color.red),
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
        public static Dictionary<Tag, BuildingDef> tileTagDict = new Dictionary<Tag, BuildingDef>();

        private static GameObject baseTemplate;

        public static void RegisterAll()
        {
            baseTemplate = Traverse.Create(BuildingConfigManager.Instance).Field<GameObject>("baseTemplate").Value;

            foreach (TileInfo info in tileInfos)
            {
                RegisterTile(info);
                Log.Debuglog("Registered tile: ", info.ID);
            }
        }

        private static void RegisterTile(TileInfo info)
        {
            info.CreateDef();

            if (info.Def is null)
            {
                tileInfos.Remove(info);
                return;
            };

            // Check for DLC
            if (!DlcManager.IsDlcListValidForCurrentContent(info.Def.RequiredDlcIds)) return;

            // Create building game object
            StainedGlassHelper.CreateFromBaseTemplate(info.Def, baseTemplate);

            info.Def.PostProcess();

            StainedGlassHelper.DoPostConfigureComplete(info.Def.BuildingComplete);
            StainedGlassHelper.DoPostConfigureUnderConstruction(info.Def.BuildingUnderConstruction);

            Assets.AddBuildingDef(info.Def);
            tileTagDict.Add(info.ElementTag, info.Def);
        }

        public class TileInfo
        {
            public string ID { get; private set; }

            public BuildingDef Def { get; private set; }

            public Tag ElementTag { get; private set; }  = Tag.Invalid;

            private Color? specColor;
            private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

            public TileInfo(Tag elementTag)
            {
                ElementTag = elementTag;
                SetID(elementTag);
            }

            public TileInfo(SimHashes element) : this(element.CreateTag()) { }

            private void SetID(Tag elementName) => ID = Mod.PREFIX + elementName.ToString() + "StainedGlassTile";

            public void CreateDef()
            {
                if (ElementLoader.GetElement(ElementTag) is null) return;

                Def = StainedGlassHelper.CreateGlassTileDef(ElementTag.ToString(), ID);

                if(specColor.HasValue)
                {
                    Def.BlockTileMaterial = new Material(Def.BlockTileMaterial);
                    Def.BlockTileMaterial.SetColor("_ShineColour", specColor.Value);
                }

                Def.RequiredDlcIds = dlcIds;
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
