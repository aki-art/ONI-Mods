using Backwalls.Buildings;
using Backwalls.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Backwalls
{
    public class Mod : UserMod2
    {
        public static BackwallRenderer renderer;
        public static bool isTrueTilesHere;
        public static List<BackwallPattern> variants = new List<BackwallPattern>();
        private static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;

        public static void SaveSettings()
        {
            config.Write();
        }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(Utils.ModPath);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            isTrueTilesHere = mods.Any(mod => mod.staticID == "TrueTiles" && mod.IsEnabledForActiveDlc());

            if (isTrueTilesHere)
            {
                ModAssets.uiSprites = new Dictionary<string, Sprite>();
                Integration.TrueTilesPatches.Patch(harmony);
            }

            Integration.Blueprints.BluePrintsPatch.TryPatch(harmony);
        }

        private static StringBuilder builder;

        private void PrintTable()
        {
            builder = new StringBuilder();

            var ores = new List<SimHashes>()
            {
                SimHashes.AluminumOre,
                SimHashes.Cobaltite,
                SimHashes.Cuprite,
                SimHashes.Electrum,
                SimHashes.GoldAmalgam,
                SimHashes.IronOre,
                SimHashes.Niobium,
                SimHashes.Steel,
                SimHashes.TempConductorSolid,
                SimHashes.UraniumOre,
                SimHashes.Wolframite
            };

            PrintPart("Aluminum Ore", SimHashes.AluminumOre.ToString(), "Mesh Tile", "default");
            PrintPart("Cobalt Ore", SimHashes.Cobaltite.ToString(), "Mesh Tile", "default");
            PrintPart("Copper Ore", SimHashes.Cuprite.ToString(), "Mesh Tile", "default");
            PrintPart("Electrum", SimHashes.Electrum.ToString(), "Mesh Tile", "default");
            PrintPart("Gold Amalgam", SimHashes.GoldAmalgam.ToString(), "Mesh Tile", "default");
            PrintPart("Iron Ore", SimHashes.IronOre.ToString(), "Mesh Tile", "default");
            PrintPart("Niobium", SimHashes.Niobium.ToString(), "Mesh Tile", "default");
            PrintPart("Steel", SimHashes.Steel.ToString(), "Mesh Tile", "default");
            PrintPart("Thermium", SimHashes.TempConductorSolid.ToString(), "Mesh Tile", "default");
            PrintPart("Uranium Ore", SimHashes.UraniumOre.ToString(), "Mesh Tile", "default");
            PrintPart("Wolframite", SimHashes.Wolframite.ToString(), "Mesh Tile", "default");

            var metals = new List<SimHashes>()
            {

                SimHashes.Aluminum,
                SimHashes.Cobalt,
                SimHashes.Copper,
                SimHashes.FoolsGold,
                SimHashes.Gold,
                SimHashes.Iron,
                SimHashes.Lead,
                SimHashes.EnrichedUranium,
                SimHashes.DepletedUranium,
                SimHashes.Tungsten
            };


        }

        private void PrintPart(string name, string element, string tile, string folder)
        {
            var imgPath = $"../../img/{folder}/{tile}_{element}_main.png".ToLowerInvariant();
            builder.AppendLine("<tr>");
            builder
                .AppendLine("\t<td>")
                .AppendLine("\t\t<img src=\"").Append(imgPath).Append("\"><")
                .AppendLine("\t</td>")

                .AppendLine("\t<td>")
                .AppendLine("\t\t").Append(name).Append(" ").Append(tile)
                .AppendLine("\t</td>");

            builder.AppendLine("</tr>");
        }
    }
}
