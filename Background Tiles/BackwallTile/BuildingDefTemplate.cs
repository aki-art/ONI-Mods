using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace BackgroundTiles.Buildings
{
    public class BuildingDefTemplate
    {
        public static BuildingDef CreateBackwallTileDef(BuildingDef original, string ID, string[] dlcIds)
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "bw_backwall_kanim",
                Mathf.FloorToInt(original.HitPoints * Mod.Settings.HitPointModifier),
                original.ConstructionTime * Mod.Settings.MassModifier,
                GetMass(original.Mass),
                original.MaterialCategory,
                original.BaseMeltingPoint,
                BuildLocationRule.NotInTiles,
                GetDecor(original.BaseDecor, 2),
                NOISE_POLLUTION.NONE
            );

            //BuildingTemplates.CreateFoundationTileDef(def);

            //def.IsFoundation = false;
            //def.TileLayer = ObjectLayer.Backwall;
            def.ReplacementLayer = ObjectLayer.Backwall;

            def.ReplacementTags = new List<Tag>
            {
                ModAssets.Tags.backWall
            };

            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;

            def.UseStructureTemperature = false;
            def.BaseTimeUntilRepair = -1f;

            def.AudioCategory = original.AudioCategory;
            def.AudioSize = "small";

            def.ObjectLayer = ObjectLayer.Backwall;
            def.SceneLayer = Grid.SceneLayer.LogicWires;//Mod.Settings.UseLogicGatesFrontSceneLayer ? Grid.SceneLayer.LogicGates : Grid.SceneLayer.Backwall;
            //def.ForegroundLayer = Grid.SceneLayer.LogicGatesFront;
            def.BlockTileIsTransparent = true;
            //def.isKAnimTile = true;

            //def.BlockTileMaterial = original.BlockTileMaterial;
            //def.BlockTileAtlas = original.BlockTileAtlas;
            //def.BlockTilePlaceAtlas = original.BlockTilePlaceAtlas; // todo: replace with custom
            //def.BlockTileShineAtlas = original.BlockTileShineAtlas;
            def.RequiredDlcIds = dlcIds;

            // leaving these null so they are not rendered
            // def.DecorBlockTileInfo = null;
            // def.DecorPlaceBlockTileInfo = null;

            return def;
        }

        private static EffectorValues GetDecor(float originalDecor, int range)
        {
            int decor = Mathf.FloorToInt(originalDecor * Mod.Settings.DecorModifier);

            if (Mod.Settings.CapDecorAt0)
            {
                decor = Mathf.Max(decor, 0);
            }

            return new EffectorValues(decor, range);
        }

        private static float[] GetMass(float[] originalMass)
        {
            float[] result = new float[originalMass.Length];
            for (int i = 0; i < originalMass.Length; i++)
            {
                result[i] = originalMass[i] * Mod.Settings.MassModifier;
                result[i] = Math.Max(result[i], 1); // need at least 1
            }

            return result;
        }
    }
}