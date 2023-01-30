using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content;
using Twitchery.Utils;

namespace Twitchery.Patches
{
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                // Add my new elements
                var list = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();
                Elements.RegisterSubstances(list);
            }

            public static void Postfix()
            {
                // fix a bug with tags, IMPORTANT FOR NEW ELEMENTS
                ElementUtil.FixTags();
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                if (DlcManager.IsExpansion1Active())
                {
                    var jello = __result.Find(e => e.elementId == Elements.Jello.ToString());
                    if (jello != null)
                    {
                        // 37?
                        jello.highTemp = GameUtil.GetTemperatureConvertedToKelvin(100, GameUtil.TemperatureUnit.Celsius);
                        jello.highTempTransitionTarget = SimHashes.Steam.ToString();
                        jello.highTempTransitionOreId = SimHashes.Sucrose.ToString();
                        jello.highTempTransitionOreMassConversion = 0.33f;
                    }
                }
            }
        }
    }
}
