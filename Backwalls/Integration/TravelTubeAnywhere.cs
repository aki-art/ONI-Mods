using FUtility;
using System;

namespace Backwalls.Integration
{
    internal class TravelTubeAnywhere
    {
        public static void OnAllBuildingsLoaded()
        {
            if(Type.GetType("TravelTubeAnywhere.TravelTubeConfig_CreateBuildingDef, TravelTubeAnywhere") != null)
            {
                Assets.GetBuildingDef(TravelTubeConfig.ID).ObjectLayer = ObjectLayer.PlasticTile; 
                Log.Info("Forced travel tubes to the unused PlasticTile layer, as Travel Tubes " +
                    "Anywhere used Backwall layer, making drywalls and backwalls incompatible with it.");
            }
        }
    }
}
