/*using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static DetailsScreen;

namespace Slag.ModUI
{
    [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
    public static class DetailsScreen_OnPrefabInit_Patch
    {
        public static void Postfix(List<SideScreenRef> ___sideScreens, GameObject ___sideScreenContentBody)
        {
            var refPanel = ___sideScreens.Find(s => s.name == "SingleButtonSideScreen");
            var screenPrefab = UnityEngine.Object.Instantiate(refPanel.screenPrefab);
            var screenPrefab2 = screenPrefab.gameObject.AddComponent<ConcreteTilesSideScreen>();
            var screen = new SideScreenRef
            {
                name = "ConcreteTilesSideScreen",
                offset = Vector2.zero,
                screenPrefab = screenPrefab2,
                screenInstance = screenPrefab2
            };
            ___sideScreens.Add(screen); 

            *//*     GameObject panel = new GameObject("ConcreteTilesSideScreen");
                 panel.transform.SetParent(___sideScreenContentBody.transform);

                 var transform = panel.AddOrGet<RectTransform>();
                 transform = refPanel.screenPrefab.gameObject.AddOrGet<RectTransform>();
                 transform.sizeDelta = new Vector2(transform.sizeDelta.x, 100);
                 panel.AddComponent<CanvasRenderer>();

                 UnityEngine.Object.Instantiate(ModAssets.pikachu, panel.transform);

                 var sideScreen = panel.AddComponent<ConcreteTilesSideScreen>();
                 sideScreen.activateOnSpawn = true;

                 var screen = new SideScreenRef
                 {
                     name = "ConcreteTilesSideScreen",
                     offset = Vector2.zero,
                     screenPrefab = sideScreen,
                     screenInstance = sideScreen
                 }; *//*

        }
    }
    class ConcreteTilesSideScreen : SingleButtonSideScreen
    {
        private ISidescreenTestControl target;
        public override string GetTitle()
        {
            return "Test Title";
        }
        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<ISidescreenTestControl>() != null;
        }

    }

    public interface ISidescreenTestControl
    {
        string SidescreenTitleKey { get; }
        string SidescreenStatusMessage { get; }
        string SidescreenButtonText { get; }
        void OnSidescreenButtonPressed();
    }
}
*/