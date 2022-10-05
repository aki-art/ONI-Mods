using FUtility;
using FUtility.FUI;
using Klei.AI;
using System;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge
{
    public class ModAssets
    {
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

        public static void LateLoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("pprechargeassets");
            var tmp = new TMPConverter();

            Prefabs.bioInkSideScreen = bundle.LoadAsset<GameObject>("BioInkSidescreen");
            Log.Assert("Bio-Ink Sidescreen", Prefabs.bioInkSideScreen);
            tmp.ReplaceAllText(Prefabs.bioInkSideScreen);

            Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
            Log.Assert("Settings Dialog", Prefabs.settingsDialog);
            tmp.ReplaceAllText(Prefabs.settingsDialog);

            StatusItems.printReady = new StatusItem(
                "ppr_printready",
                "BUILDINGS.",
                "status_item_doubleexclamation",
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID);
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
