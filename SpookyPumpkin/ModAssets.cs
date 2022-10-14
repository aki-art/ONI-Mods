

using FUtility;
using FUtility.FUI;
using Newtonsoft.Json;
using SpookyPumpkinSO.Content;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpookyPumpkinSO
{
    internal class ModAssets
    {
        public static readonly Tag buildingPumpkinTag = TagManager.Create("SP_BuildPumpkin", STRINGS.ITEMS.FOOD.SP_PUMPKIN.NAME);
        public static HashSet<Tag> pipTreats;

        public static class SnapOns
        {
            public const string SKELLINGTON = "SP_JackSkellingtonCostume_SnapOn";
            public const string SKELLINGTON_CHEEK = "SP_JackSkellingtonCostume_SnapOn_Cheek";

            public static Dictionary<string, string[]> Lookup = new Dictionary<string, string[]>()
            {
                {
                    SPEquippableFacades.SKELLINGTON, new[]
                    {
                        SKELLINGTON,
                        SKELLINGTON_CHEEK
                    }
                }
            };
        }

        public class Prefabs
        {
            public static GameObject sideScreenPrefab;
            // custom settings UI is disabled for now
            // public static GameObject settingsDialogPrefab;
        }

        public static class Colors
        {
            public static Color pumpkinOrange = Util.ColorFromHex("ffa63b");
        }

        public static string ModPath { get; internal set; }

        internal static void LateLoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("sp_uiasset");

            Prefabs.sideScreenPrefab = bundle.LoadAsset<GameObject>("GhostPipSideScreen");
            //Prefabs.settingsDialogPrefab = bundle.LoadAsset<GameObject>("SpookyOptions");
            var tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.sideScreenPrefab, false);
            //TMPConverter.ReplaceAllText(Prefabs.settingsDialogPrefab);
        }

        public static void ReadTreats()
        {
            pipTreats = new HashSet<Tag>();
            foreach (var treat in ReadPipTreats())
            {
                var item = Assets.TryGetPrefab(treat);
                if (item != null && item.GetComponent<Pickupable>() != null)
                {
                    pipTreats.Add(treat);
                }
            }

            if (pipTreats.Count == 0)
            {
                pipTreats.Add(GrilledPrickleFruitConfig.ID);
            }
        }

        public static List<string> ReadPipTreats()
        {
            if (ReadJSON("piptreats", out var json))
            {
                return JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                return new List<string>();
            }
        }

        private static bool ReadJSON(string filename, out string json, bool log = true)
        {
            json = null;
            try
            {
                using (var r = new StreamReader(Path.Combine(ModPath, filename + ".json")))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                if (log)
                {
                    Log.Warning($"Couldn't read {filename}.json, {e.Message}. Using defaults.");
                }

                return false;
            }

            return true;
        }
    }
}
