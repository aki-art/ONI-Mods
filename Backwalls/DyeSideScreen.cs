using Backwalls.Buildings;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Backwalls
{
    public class DyeSideScreen : SideScreenContent
    {
        public static List<Color> colors = new List<Color>();
        static ColorToggle togglePrefab;
        Transform content;

        protected override void OnPrefabInit()
        {
            Log.Debuglog("prefab init");
            base.OnPrefabInit();



        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if(colors == null)
            {
                var t_detailsScreen = Traverse.Create(DetailsScreen.Instance);
                var screens = t_detailsScreen.Field("sideScreens").GetValue<List<DetailsScreen.SideScreenRef>>();

                foreach (var screen in screens)
                {
                    if (screen.name == "PixelPack SideScreen")
                    {
                        colors = Traverse.Create(screen).Field<List<Color>>("colorSwatch").Value;
                        return;
                    }
                }
            }

        }

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<IDyable>() != null;

        class ColorToggle : Toggle
        {
            public Color color;

            public override void OnPointerClick(PointerEventData eventData)
            {
                base.OnPointerClick(eventData);
                Log.Debuglog(color);
            }
        }
    }
}
