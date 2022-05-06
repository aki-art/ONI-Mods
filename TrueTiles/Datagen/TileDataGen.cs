using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrueTiles.Datagen
{
    // Used to generate my json before release, and when the user manually "resets" mod data
    public class TileDataGen : DataGen
    {
        private const string TILE = "tile";
        private const string CARPET = "carpet";
        private const string INSULATION = "insulation";
        private const string MESH = "mesh";
        private const string AIRFLOW = "airflow";
        private const string METAL = "metal";
        private const string WINDOW = "window";

        public TileDataGen(string path) : base(path)
        {
            WriteTiles("Default", ConfigureDefaultTiles());
            WriteTiles("Material", ConfigureDefaultTiles()); // identical overrides so this works
            WriteTiles("BeautifulGranite", ConfigureBeautifulGranite());
            WriteTiles("CutesyCarpet", ConfigureCutesyCarpet());
        }

        private Dictionary<string, Dictionary<string, TileData>> ConfigureDefaultTiles()
        {
            var tiles = new Dictionary<string, Dictionary<string, TileData>>();

            AddTiles(tiles, TileConfig.ID)
                .AddSimpleTile(TILE, SimHashes.Ceramic.ToString(), false)
                .AddSimpleTile(TILE, SimHashes.Fossil.ToString())
                .AddSimpleTile(TILE, SimHashes.Granite.ToString(), false)
                .AddSimpleTile(TILE, SimHashes.Graphite.ToString())
                .AddSimpleTile(TILE, SimHashes.IgneousRock.ToString())
                .AddSimpleTile(TILE, SimHashes.Isoresin.ToString())
                .AddSimpleTile(TILE, SimHashes.MaficRock.ToString())
                .AddSimpleTile(TILE, SimHashes.Obsidian.ToString())
                .AddSimpleTile(TILE, SimHashes.SandStone.ToString())
                .AddSimpleTile(TILE, SimHashes.SedimentaryRock.ToString())
                .AddSimpleTile(TILE, SimHashes.SuperInsulator.ToString());

            AddTiles(tiles, CarpetTileConfig.ID)
                .AddSimpleTile(CARPET, SimHashes.Ceramic.ToString())
                .AddSimpleTile(CARPET, SimHashes.Fossil.ToString())
                .AddSimpleTile(CARPET, SimHashes.Granite.ToString())
                .AddSimpleTile(CARPET, SimHashes.Graphite.ToString())
                .AddSimpleTile(CARPET, SimHashes.IgneousRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.Isoresin.ToString())
                .AddSimpleTile(CARPET, SimHashes.MaficRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.Obsidian.ToString())
                .AddSimpleTile(CARPET, SimHashes.SedimentaryRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.SuperInsulator.ToString());

            AddTiles(tiles, InsulationTileConfig.ID)
                .AddSimpleTile(INSULATION, SimHashes.Ceramic.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.Fossil.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.Granite.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.Graphite.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.IgneousRock.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.Isoresin.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.MaficRock.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.Obsidian.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.SedimentaryRock.ToString(), false)
                .AddSimpleTile(INSULATION, SimHashes.SuperInsulator.ToString(), false);

            AddTiles(tiles, MeshTileConfig.ID)
                .AddShinyTile(MESH, SimHashes.AluminumOre.ToString(), false)
                .AddShinyTile(MESH, SimHashes.Cobaltite.ToString(), false)
                .AddShinyTile(MESH, SimHashes.Cuprite.ToString(), false)
                .AddShinyTile(MESH, SimHashes.Electrum.ToString(), false)
                .AddShinyTile(MESH, SimHashes.GoldAmalgam.ToString(), false)
                .Add(SimHashes.IronOre, new TileDataBuilder(MESH, SimHashes.IronOre, false)
                    .Build())
                .AddShinyTile(MESH, SimHashes.Niobium.ToString(), false, false)
                .AddShinyTile(MESH, SimHashes.Steel.ToString(), false)
                .AddShinyTile(MESH, SimHashes.TempConductorSolid.ToString(), false)
                .AddShinyTile(MESH, SimHashes.UraniumOre.ToString(), false)
                .AddShinyTile(MESH, SimHashes.Wolframite.ToString(), false)
                .Add(SimHashes.Aluminum, new TileDataBuilder(MESH, SimHashes.AluminumOre, false)
                    .Specular("mesh_aluminumore_spec", "mesh_aluminumore_top")
                    .Build())
                .Add(SimHashes.Cobalt, new TileDataBuilder(MESH, SimHashes.Cobaltite, false)
                    .Specular("mesh_cobaltite_spec", "mesh_cobaltite_top")
                    .Build())
                .Add(SimHashes.Copper, new TileDataBuilder(MESH, SimHashes.Cuprite, false)
                    .Specular("mesh_cuprite_spec", "mesh_cuprite_top")
                    .Build())
                .Add(SimHashes.Gold, new TileDataBuilder(MESH, SimHashes.GoldAmalgam, false)
                    .Specular("mesh_goldamalgam_spec", "mesh_goldamalgam_top")
                    .Build())
                .Add(SimHashes.FoolsGold, new TileDataBuilder(MESH, SimHashes.GoldAmalgam, false)
                    .Specular("mesh_goldamalgam_spec", "mesh_goldamalgam_top")
                    .Build())
                .Add(SimHashes.Iron, new TileDataBuilder(MESH, SimHashes.IronOre, false)
                    .Build())
                .AddShinyTile(MESH, SimHashes.Lead.ToString(), false)
                .Add(SimHashes.DepletedUranium, new TileDataBuilder(MESH, SimHashes.UraniumOre, false)
                    .Specular("mesh_uraniumore_spec", "mesh_uraniumore_top")
                    .Build())
                .Add(SimHashes.EnrichedUranium, new TileDataBuilder(MESH, SimHashes.UraniumOre, false)
                    .Specular("mesh_uraniumore_spec", "mesh_uraniumore_top")
                    .SpecularColor(Color.green).Build())
                .Add(SimHashes.Tungsten, new TileDataBuilder(MESH, SimHashes.Wolframite, false)
                    .Specular("mesh_wolframite_spec", "mesh_wolframite_top")
                    .Build());

            AddTiles(tiles, GasPermeableMembraneConfig.ID)
                .AddSimpleTile(AIRFLOW, SimHashes.AluminumOre.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Cobaltite.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Cuprite.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Electrum.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.GoldAmalgam.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.IronOre.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Niobium.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Steel.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.TempConductorSolid.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.UraniumOre.ToString(), false)
                .AddSimpleTile(AIRFLOW, SimHashes.Wolframite.ToString(), false)
                .Add(SimHashes.Aluminum, new TileDataBuilder(AIRFLOW, SimHashes.AluminumOre, false).Build())
                .Add(SimHashes.Cobalt, new TileDataBuilder(AIRFLOW, SimHashes.Cobaltite, false).Build())
                .Add(SimHashes.Copper, new TileDataBuilder(AIRFLOW, SimHashes.Cuprite, false).Build())
                .Add(SimHashes.Gold, new TileDataBuilder(AIRFLOW, SimHashes.GoldAmalgam, false).Build())
                .Add(SimHashes.FoolsGold, new TileDataBuilder(AIRFLOW, SimHashes.GoldAmalgam, false).Build())
                .Add(SimHashes.Iron, new TileDataBuilder(AIRFLOW, SimHashes.IronOre, false).Build())
                .AddShinyTile(AIRFLOW, SimHashes.Lead.ToString(), false)
                .Add(SimHashes.DepletedUranium, new TileDataBuilder(AIRFLOW, SimHashes.UraniumOre, false).Build())
                .Add(SimHashes.EnrichedUranium, new TileDataBuilder(AIRFLOW, SimHashes.UraniumOre, false)
                    .SpecularColor(Color.green).Build())
                .Add(SimHashes.Tungsten, new TileDataBuilder(AIRFLOW, SimHashes.Wolframite, false).Build());

            AddTiles(tiles, MetalTileConfig.ID)
                .AddShinyTile(METAL, SimHashes.Aluminum.ToString(), false, false)
                .AddShinyTile(METAL, SimHashes.Cobalt.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Copper.ToString(), false)
                .AddShinyTile(METAL, SimHashes.DepletedUranium.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Gold.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Iron.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Lead.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Niobium.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Steel.ToString(), false)
                .AddShinyTile(METAL, SimHashes.TempConductorSolid.ToString(), false)
                .AddShinyTile(METAL, SimHashes.Tungsten.ToString(), false);

            AddTiles(tiles, GlassTileConfig.ID)
                .AddShinyTile(WINDOW, SimHashes.Diamond.ToString());

            return tiles;
        }
        private Dictionary<string, Dictionary<string, TileData>> ConfigureCutesyCarpet()
        {
            var tiles = new Dictionary<string, Dictionary<string, TileData>>();

            AddTiles(tiles, CarpetTileConfig.ID)
                .AddSimpleTile(CARPET, SimHashes.Ceramic.ToString())
                .AddSimpleTile(CARPET, SimHashes.Fossil.ToString())
                .AddSimpleTile(CARPET, SimHashes.Granite.ToString())
                .AddSimpleTile(CARPET, SimHashes.Graphite.ToString())
                .AddSimpleTile(CARPET, SimHashes.IgneousRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.Isoresin.ToString())
                .AddSimpleTile(CARPET, SimHashes.MaficRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.Obsidian.ToString())
                .AddSimpleTile(CARPET, SimHashes.SedimentaryRock.ToString())
                .AddSimpleTile(CARPET, SimHashes.SuperInsulator.ToString());

            return tiles;
        }

        private Dictionary<string, Dictionary<string, TileData>> ConfigureBeautifulGranite()
        {
            var tiles = new Dictionary<string, Dictionary<string, TileData>>();

            AddTiles(tiles, TileConfig.ID)
                .Add(SimHashes.Granite, new TileDataBuilder(TILE, SimHashes.Granite, false)
                    .Specular("tile_granite_spec", "tile_granite_spec_top")
                    .Normal("granite_normal")
                    .SpecularColor(Color.white * 5f)
                    .Build());

            return tiles;
        }

        private ElementOverrides AddTiles(Dictionary<string, Dictionary<string, TileData>> tiles, string id)
        {
            var overrides = new ElementOverrides(id);
            tiles.Add(id, overrides.items);
            return overrides;
        }

        private void WriteTiles(string id, Dictionary<string, Dictionary<string, TileData>> data)
        {
            var tilesPath = FileUtil.GetOrCreateDirectory(Path.Combine(path, id));
            Write(tilesPath, "tiles", data);
        }
    }
}
