

using FUtility;
using FUtility.FUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpookyPumpkinSO
{
    internal class ModAssets
    {
        public const string spookedEffectID = "SP_Spooked";
        public const string holidaySpiritEffectID = "AHM_HolidaySpirit";
        public static readonly Tag buildingPumpkinTag = TagManager.Create("SP_BuildPumpkin", STRINGS.ITEMS.FOOD.SP_PUMPKIN.NAME);
        public static HashSet<Tag> pipTreats;

        public class Prefabs
        {
            public static GameObject sideScreenPrefab;
            // custom settings UI is disabled for now
            // public static GameObject settingsDialogPrefab;
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
