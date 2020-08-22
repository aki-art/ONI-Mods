using FUtility.FUI;
using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CenterOverlay
{
    class OverlayPatches
    {
        [HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_Patch
        {
            private static bool IsOnModdedSide(Vector3 position)
            {
                var midPoint = Grid.WidthInCells / 2 + ModAssets.Offset;
                return (Grid.PosToXY(position).x > midPoint) ^ ModAssets.moddedOnLeft;
            }

            public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
            {
                ___getColourFuncs.Add(SymmetryMode.ID, GetMirrorColor);
            }

            private static Color GetMirrorColor(SimDebugView instance, int cell)
            {
                Color color = ModAssets.vanillaColor;
                if (IsOnModdedSide(Grid.CellToPos(cell)) || HasModdedBuilding(cell))
                    color = ModAssets.moddedColor;

                return color;
            }

            private static bool HasModdedBuilding(int cell)
            {
                GameObject gameObject = Grid.Objects[cell, (int)ObjectLayer.Building];
                return Grid.IsValidCell(cell) && gameObject != null && gameObject.HasTag(ModAssets.ModdedBuilding);
            }
        }

        [HarmonyPatch(typeof(OverlayLegend), "PopulateOverlayInfoUnits")]
        public static class OverlayLegend_PopulateOverlayInfoUnits_Patch
        {
            public static void Postfix(OverlayLegend.OverlayInfo overlayInfo, List<GameObject> ___activeDiagrams)
            {
                if(overlayInfo.mode == SymmetryMode.ID)
                {
                    foreach(GameObject diagram in ___activeDiagrams)
                    {
                        if(diagram.name.Contains(ModAssets.overlaySettingsButtonPrefab.name))
                        {
                            FButton button = diagram.transform.Find("Panel/SettingsButton").gameObject.AddComponent<FButton>();
                            button.OnClick += () => Helper.OpenFDialog<SettingsScreen>(ModAssets.settingsScreenPrefab);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(OverlayLegend), "OnSpawn")]
        public static class OverlayLegend_OnSpawn_Patch
        {
            public static void Postfix(List<OverlayLegend.OverlayInfo> ___overlayInfoList)
            {
                ___overlayInfoList.Add(SymmetryInfo());
            }

            private static OverlayLegend.OverlayInfo SymmetryInfo()
            {
                Sprite box = Assets.GetSprite("web_box");
                return new OverlayLegend.OverlayInfo
                {
                    name = "Symmetry overlay",
                    mode = SymmetryMode.ID,
                    isProgrammaticallyPopulated = false,
                    infoUnits = new List<OverlayLegend.OverlayInfoUnit>()
                    {
                        new OverlayLegend.OverlayInfoUnit(box, "Modded", ModAssets.moddedColor, Color.white),
                        new OverlayLegend.OverlayInfoUnit(box, "Vanilla", ModAssets.vanillaColor, Color.white)
                    },
                    diagrams = new List<GameObject>
                    {
                        ModAssets.overlaySettingsButtonPrefab
                    }
                };
            }
        }


        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles_Patch
        {
            public static void Postfix(ref List<KIconToggleMenu.ToggleInfo> ___overlayToggleInfos)
            {

                Type OverlayToggleInfo = AccessTools.Inner(typeof(OverlayMenu), "OverlayToggleInfo");

                var OverlayToggleInfoConstructor = OverlayToggleInfo.GetConstructor(
                    new Type[] {
                        typeof(string),
                        typeof(string),
                        typeof(HashedString),
                        typeof(string),
                        typeof(Action),
                        typeof(string),
                        typeof(string) });

                object[] args = new object[] {
                        "Toggle Symmetry Check",
                        "overlay_symmetry",
                        SymmetryMode.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object newToggle = OverlayToggleInfoConstructor.Invoke(args);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)newToggle);
            }
        }

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                var StatusItemT = Traverse.Create(typeof(StatusItem));
                var fieldMap = StatusItemT.Field("overlayBitfieldMap").GetValue<Dictionary<HashedString, StatusItem.StatusItemOverlays>>();
                fieldMap.Add(SymmetryMode.ID, StatusItem.StatusItemOverlays.None);
            }
        }

        [HarmonyPatch(typeof(OverlayScreen), "RegisterModes")]
        public static class OverlayScreen_RegisterModes_Patch
        {
            public static void Postfix()
            {
                Traverse overlayScreen = Traverse.Create(OverlayScreen.Instance);
                overlayScreen.Method("RegisterMode", new SymmetryMode()).GetValue();
            }
        }
    }
}
