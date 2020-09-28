using FUtility;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTrashBin
{
    class Patches
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                GameTags.UnitCategories.Add(GameTags.Artifact);
                GameTags.UnitCategories.Add(GameTags.MiscPickupable);
                Buildings.RegisterSingleBuilding(typeof(SmartTrashBinConfig));
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Strings.Add("STRINGS.MISC.TAGS.MISCPICKUPABLE", "Miscallenous");
                //Loc.Translate(typeof(STRINGS));
            }
        }


/*        [HarmonyPatch(typeof(FilteredStorage), "OnFilterChanged")]
        public static class FilteredStorage_OnFilterChanged_Patch
        {
            public static void Prefix(KMonoBehaviour ___root, Tag[] tags, Tag[] ___forbiddenTags)
            {
                if(___root != null && ___root is SmartTrashBin)
                {
                    SmartTrashBin bin = ___root as SmartTrashBin;
                    bin.UpdateFilters(tags);
                }
            }

            public static void Postfix()
            {

            }
        }*/

    }
}
