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
                    TweakAsphaltSpeed(Tuning.ConvertSpeed(ModSettings.Asphalt.SpeedMultiplier));
            }

            private static void TweakAsphaltSpeed(float speed)
            {
                if(Assets.GetPrefab(AsphaltTileConfig.ID).TryGetComponent(out SimCellOccupier sco))
                    sco.movementSpeedMultiplier = speed;
            }
        }

    }
}
