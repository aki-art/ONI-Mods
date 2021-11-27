using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BackgroundTiles.Patches
{
    class PlanScreenPatch
    {
        [HarmonyPatch(typeof(PlanScreen), "RefreshBuildingButton")]
        public static class PlanScreen_RefreshBuildingButton_Patch
        {
            // Happens once per game launch, persistent between world loads
            public static void Postfix(BuildingDef def, KToggle toggle, HashedString buildingCategory)
            {
                if (BackgroundTilesManager.IsBackwall(def))
                {
                    /*
                    Image image = toggle.bgImage.GetComponentsInChildren<Image>()[1];
                    Swatch swatch = image.gameObject.AddOrGet<Swatch>();
                    FUtility.Log.Assert("swatch", swatch);
                    swatch.SetSprite(def);*/
                    //Image swatch = Object.Instantiate(image, image.transform);
                    //swatch.transform.localScale = new Vector3(100, 100);
                    //swatch.transform.localPosition += new Vector3(-20, -20);
                   //swatch.sprite = BackgroundTilesManager.GetSprite(def);
                }
            }
        }
    }
}
