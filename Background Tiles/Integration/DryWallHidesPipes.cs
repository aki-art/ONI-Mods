using System;

namespace BackgroundTiles.Integration
{
    public class DryWallHidesPipes
    {
        public static void Check()
        {
            if(Type.GetType("DrywallHidesPipes.DrywallPatch, DrywallHidesPipes", false, false) is object)
            {
                Mod.Settings.UseLogicGatesFrontSceneLayer = true;
                FUtility.Log.Info("Drywall Hides Pipes compatibility initialized.");
            }
        }
    }
}
