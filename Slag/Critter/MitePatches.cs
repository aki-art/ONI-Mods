using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utils;

namespace Slag.Critter
{
    class MitePatches
    {

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                LoadMiteRewards();
            }
        }

        public static void LoadMiteRewards()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "mite_rewards.json");
            Log.Debuglog("Loading mite reward file from: " + filePath);

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    MiteTuning.moltWeights = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, float>>>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}.");
            }
        }
        //List<Element> rawMetal = ElementLoader.elements.FindAll((Element e) => e.IsSolid && e.HasTag(GameTags.Metal));
    }
}
