using FUtility;
using HarmonyLib;
using UnityEngine;
using ZipLine.Buildings.ZiplinePost;
using ZipLine.Tools;

namespace ZipLine.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public class Game_OnSpawn_Patch
        {
            public static void Postfix(Game __instance)
            {
                var tetherGo = new GameObject("Ziplines Tether Visualizer");
                tetherGo.transform.parent = __instance.transform;
                tetherGo.AddComponent<TetherVisualizer>().tetherOffset = ZiplineAnchor.offset;
                tetherGo.SetActive(true);

                Game.Instance.gameObject.AddComponent<ZipConnectorTool>();
            }
        }
    }
}
