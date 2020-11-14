using Harmony;

namespace Asphalt
{
    public class AsphaltSpeedPatch
    {
        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public static class Game_OnSpawn_Patch
        {
            public static void Postfix()
            {
                if (ModSettings.speedChanged)
                    TweakAsphaltSpeed(Tuning.FormatSpeed(ModSettings.Asphalt.SpeedMultiplier));
            }

            private static void TweakAsphaltSpeed(float speed)
            {
                SimCellOccupier sco = Assets.GetPrefab(AsphaltTileConfig.ID).GetComponent<SimCellOccupier>();
                if (sco != null) 
                    sco.movementSpeedMultiplier = speed;
            }
        }

    }
}
