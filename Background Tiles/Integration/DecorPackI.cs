using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BackgroundTiles.Integration
{
    public class DecorPackI
    {
        public static Dictionary<Tag, Tag> tileToWallLookup = new Dictionary<Tag, Tag>();
        public static Dictionary<Tag, Tag> elementToWallLookup = new Dictionary<Tag, Tag>();
        public static string defaultID = "BackgroundTiles_DecorPackA_DefaultStainedGlassTileWall";
        public static bool decorPackExists = false;

        public static void Init(Harmony harmony)
        {
            Type StainedGlassTilesType = Type.GetType("DecorPackA.Buildings.StainedGlassTile.StainedGlassTiles, DecorPackA", false, false);
            if (StainedGlassTilesType is object)
            {
                decorPackExists = true;

                MethodInfo registerAll = StainedGlassTilesType.GetMethod("RegisterAll", BindingFlags.Public | BindingFlags.Static);
                MethodInfo registerAllPostfix = typeof(StainedGlassTiles_RegisterAll_Patch).GetMethod("Postfix", types: new Type[] {typeof(Dictionary<Tag, Tag>) });
                harmony.Patch(registerAll, null, new HarmonyMethod(registerAllPostfix));
;
                MethodInfo onClickCopyBuilding = AccessTools.Method(typeof(PlanScreen), "OnClickCopyBuilding");
                MethodInfo onClickCopyBuildingPrefix = typeof(PlanScreen_OnClickCopyBuilding_Patch).GetMethod("Prefix");
                harmony.Patch(onClickCopyBuilding, new HarmonyMethod(onClickCopyBuildingPrefix));

                MethodInfo activate = AccessTools.Method(typeof(BuildTool), "Activate");
                MethodInfo activatePrefix = typeof(BuildTool_Activate_Patch).GetMethod("Prefix");
                harmony.Patch(activate, new HarmonyMethod(activatePrefix));

                Log.Info("Decor Pack I compatibility initialized.");
            }
        }

        public static class BuildTool_Activate_Patch
        {
            public static void Prefix(BuildTool __instance, ref BuildingDef def, IList<Tag> selected_elements)
            {
                if (def.PrefabID == defaultID)
                {
                    RemoveVisualizer(__instance);
                    if (elementToWallLookup.TryGetValue(selected_elements[1], out Tag buildingTag))
                    {
                        def = Assets.GetBuildingDef(buildingTag.ToString());
                    }
                }
            }

            // this prevents ghost preview blocks from appearing
            private static void RemoveVisualizer(BuildTool __instance)
            {
                if (__instance.visualizer != null)
                {
                    Traverse.Create(__instance).Method("ClearTilePreview").GetValue();
                    UnityEngine.Object.Destroy(__instance.visualizer);
                }
            }
        }

        public static Dictionary<Tag, Tag> elementToTileLookup;

        public static class StainedGlassTiles_RegisterAll_Patch
        {
            public static void Postfix(Dictionary<Tag, Tag> ___tileTagDict)
            {
                elementToTileLookup = ___tileTagDict;
            }
        }

        public static void AddDecorPackTiles()
        {
            if (elementToTileLookup is null) return;

            foreach (var tileVersion in elementToTileLookup)
            {
                BuildingDef def = Assets.GetBuildingDef(tileVersion.Value.ToString());
                if (def != null && BackgroundTilesManager.Instance.tiles.ContainsValue(def))
                {
                    BuildingDef wallVersion = BackgroundTilesManager.Instance.tiles.FirstOrDefault(d => d.Value == def).Key;
                    tileToWallLookup.Add(tileVersion.Key, wallVersion.Tag);
                    elementToWallLookup.Add(tileVersion.Key, wallVersion.Tag);
                } 
            }
        }

        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix()
            {
                if (SelectTool.Instance.selected is object)
                {
                    if (SelectTool.Instance.selected.TryGetComponent(out Building building))
                    {
                        if (elementToWallLookup is object && elementToWallLookup.ContainsValue(building.Def.Tag.ToString()) && building.Def.Tag != defaultID)
                        {
                            OpenBuildMenu(building);
                        }
                    }
                }
            }

            // Open build menu to a specific Building type
            private static void OpenBuildMenu(Building building)
            {
                foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
                {
                    if (planInfo.data.Contains(defaultID))
                    {
                        BuildingDef defaultStainedDef = Assets.GetBuildingDef(defaultID);

                        PlanScreen.Instance.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
                        PlanScreen.Instance.OnSelectBuilding(PlanScreen.Instance.ActiveToggles[defaultStainedDef].gameObject, defaultStainedDef);

                        Traverse<ProductInfoScreen> infoScreen = Traverse.Create(PlanScreen.Instance).Field<ProductInfoScreen>("productInfoScreen");

                        if (infoScreen == null) return;

                        infoScreen.Value.materialSelectionPanel.SelectSourcesMaterials(building);

                        if (building.TryGetComponent(out Rotatable rotatable))
                        {
                            BuildTool.Instance.SetToolOrientation(rotatable.GetOrientation());
                        }

                        return;
                    }
                }
            }
        }
    }
}
