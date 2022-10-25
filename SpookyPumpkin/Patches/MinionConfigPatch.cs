using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
    public class MinionConfigPatch
    {
        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(ref GameObject __result)
            {
                var snapPoints = __result.GetComponent<SnapOn>().snapPoints;

                snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = ModAssets.SnapOns.SKELLINGTON,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim("sp_skellington_mouth_kanim"),
                    overrideSymbol = "snapTo_mouth"
                });

                snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = ModAssets.SnapOns.SCARECROW_MOUTH,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim("sp_scarecrow_mouth_kanim"),
                    overrideSymbol = "snapTo_mouth"
                });

                snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = ModAssets.SnapOns.SKELLINGTON_CHEEK,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim("sp_skellingtonfacepaint_kanim"),
                    overrideSymbol = "snapTo_cheek"
                });

                __result.AddComponent<FacePaint>();
            }
        }
    }
}