using FUtility;
using FUtility.FUI;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TUNING;
using UnityEngine;

namespace PrintingPodRecharge
{
    public class ModAssets
    {
        public static HashSet<string> goodTraits;
        public static HashSet<string> badTraits;
        public static HashSet<string> needTraits;
        public static HashSet<string> vacillatorTraits;

        public static class Sounds
        {
            public static List<string> diceRolls = new List<string>()
            {
                "BioInks_DiceRoll_A",
                "BioInks_DiceRoll_B"
            };
        }

        public static string GetRootPath()
        {
            return Path.Combine(KMod.Manager.GetDirectory(), "config", "PrintingPodRecharge");
        }

        public static class Tags
        {
            public static Tag bioInk = TagManager.Create("ppr_bioink");
        }

        public static class Prefabs
        {
            public static GameObject bioInkSideScreen;
            public static GameObject settingsDialog;
        }

        public static class StatusItems
        {
            public static StatusItem printReady;
        }

        public static class Colors
        {
            public static Color gold = Util.ColorFromHex("ffdb6e");
            public static Color purple = Util.ColorFromHex("a961f9");
            public static Color magenta = Util.ColorFromHex("fd43ff");
        }

        public static void LateLoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("pprechargeassets", platformSpecific: true);
            var tmp = new TMPConverter();

            Prefabs.bioInkSideScreen = bundle.LoadAsset<GameObject>("BioInkSidescreen");
            tmp.ReplaceAllText(Prefabs.bioInkSideScreen);

            Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
            tmp.ReplaceAllText(Prefabs.settingsDialog);

            StatusItems.printReady = new StatusItem(
                "ppr_printready",
                "BUILDINGS",
                "status_item_doubleexclamation",
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID);


            var path = Path.Combine(Utils.ModPath, "assets");

            AudioUtil.LoadSound(Sounds.diceRolls[0], Path.Combine(path, "353974__nettimato__rolling-dice-2.wav"));
            AudioUtil.LoadSound(Sounds.diceRolls[1], Path.Combine(path, "353975__nettimato__rolling-dice-1.wav"));
        }

        public static bool TryReadFile(string path, out string result)
        {
            try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning($"Tried to read file at {path}, but could not be read: ", e.Message);
                result = null;
                return false;
            }
        }
    }
}
