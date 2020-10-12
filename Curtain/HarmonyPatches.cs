using FUtility;
using Harmony;
using System.Collections.Generic;
using static FUtility.FUI.SideScreen;

namespace Curtain
{
    class HarmonyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(typeof(PlasticCurtainConfig));
            }

        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                AddClonedSideScreen<CurtainSideScreen>("Curtain Side Screen", "Door Toggle Side Screen", typeof(DoorToggleSideScreen));
            }
        }

        [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModAssets.MakeStatusItem();
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {

            public static void Postfix()
            {
                List<SimHashes> elementsToTag = new List<SimHashes>
                {
                    SimHashes.Polypropylene,
                    SimHashes.Isoresin,
                    SimHashes.SuperInsulator,
                    SimHashes.SolidViscoGel
                };

                foreach (SimHashes hash in elementsToTag)
                {
                    Element element = ElementLoader.FindElementByHash(hash);
                    element.oreTags = element.oreTags.Append(ModAssets.plasticTag);
                }
            }
        }
    }
}
