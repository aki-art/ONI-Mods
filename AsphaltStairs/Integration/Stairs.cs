using AsphaltStairs.Buildings;
using HarmonyLib;
using System;
using UnityEngine;

namespace AsphaltStairs.Integration
{
    public class Stairs
    {
        public static void TryPatch(Harmony harmony)
        {
            var stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
            if (stairsType == null)
            {
                return;
            }

            var original = stairsType.GetMethod("Postfix");
            var postfix = typeof(Stairs).GetMethod("Postfix");
            harmony.Patch(original, new HarmonyMethod(postfix));
        }

        public static void Postfix(Navigator navigator, Navigator.ActiveTransition transition)
        {
            if (transition.navGridTransition.isCritter)
            {
                return;
            }

            var cell = Grid.CellBelow(Grid.PosToCell(navigator));
            var tile = Grid.Objects[cell, (int)ObjectLayer.LadderTile];

            if (tile != null)
            {
                if (tile.GetComponent<KPrefabID>().HasTag(AsphaltStairsConfig.ID))
                {
                    transition.speed *= Asphalt.speedModifier;
                    transition.animSpeed *= Asphalt.speedModifier;
                }
            }
        }

        private static void OpenStairsWorkshop()
        {
            Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=2012810337");
        }
    }
}
