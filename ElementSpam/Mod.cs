using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using UnityEngine;

namespace ElementSpam
{
    public class Mod : UserMod2
    {
        public const int COUNT = 60;
        internal static List<Substance> substanceList;
        public static List<SimHashes> simHashes = new List<SimHashes>();
        internal static AccessTools.FieldRef<SubstanceTable, List<Substance>> ref_substanceList;

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                var elementList = new List<ElementLoader.ElementEntry>();

                Log.Debuglog("Adding elements");

                for (var i = 0; i < COUNT; i++)
                {
                    //Log.Debuglog("Adding element " + i);
                    var local = "STRINGS.ELEMENTS.TESTS.TEST_" + i;
                    var name = "Test " + i;
                    Strings.Add(local, name);

                    var newElement = new ElementLoader.ElementEntry
                    {
                        elementId = "TestElement" + i,
                        state = Element.State.Liquid,
                        dlcId = DlcManager.VANILLA_ID,
                        localizationID = local,
                        isDisabled = false
                    };

                    simHashes.Add(EnumPatch.RegisterSimHash(newElement.elementId));
                    elementList.Add(newElement);
                };

                Log.Debuglog("Added elements " + elementList.Count);

                __result.AddRange(elementList);

                var animFile = Assets.Anims.Find(a => a.name == "liquid_tank_kanim");

                foreach (var item in simHashes)
                {
                    var color = Random.ColorHSV();
                    var substance = ModUtil.CreateSubstance(item.ToString(), Element.State.Liquid, animFile, Assets.instance.substanceTable.liquidMaterial, color, color, color);
                    substanceList.Add(substance);
                }
            }

            [HarmonyPatch(typeof(ElementLoader), "Load")]
            public class ElementLoader_Load_Patch
            {
                public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
                {
                    substanceList = Traverse.Create(substanceTablesByDlc[DlcManager.VANILLA_ID]).Field<List<Substance>>("list").Value;
                }
            }
        }
    }
}
