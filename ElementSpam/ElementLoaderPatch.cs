using FUtility;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementSpam
{
    public class ElementLoaderPatch
    {
        public const int COUNT = 1;
        internal static Hashtable substanceList;

        public static List<SimHashes> simHashes = new List<SimHashes>();

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                var elementList = new List<ElementLoader.ElementEntry>();

                for (var i = 0; i < COUNT; i++)
                {
                    Log.Debuglog("Adding element " + i);
                    var local = "STRINGS.ELEMENTS.TESTS.TEST_" + i;
                    var name = "Test " + i;
                    Strings.Add(local, name);

                    var newElement = new ElementLoader.ElementEntry
                    {
                        elementId = "TestElement" + i,
                        state = Element.State.Solid,
                        dlcId = DlcManager.VANILLA_ID,
                        localizationID = local,
                        isDisabled = false
                    };

                    simHashes.Add(EnumPatch.RegisterSimHash(name));
                    elementList.Add(newElement);
                };

                __result.AddRange(elementList);

                RegisterSubstances(substanceList);
            }

            private static void RegisterSubstances(Hashtable substanceList)
            {
                foreach(var item in simHashes)
                {
                    Log.Debuglog("adding mat to " + item);
                    var value = ElementUtil.CreateSubstance(item, "glass_kanim", Element.State.Solid, Color.red);
                    substanceList.Add(item, value);

                    Log.Assert("material", value.material);
                }
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Prefix(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                ElementLoaderPatch.substanceList = substanceList;
            }

            public static void Postfix(Hashtable substanceList)
            {
                var oreMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Cuprite).material;
                foreach (var item in simHashes)
                {
                    var substance = ElementLoader.FindElementByHash(item).substance;
                    substance.material = new Material(oreMaterial);
                }
            }
        }
    }
}