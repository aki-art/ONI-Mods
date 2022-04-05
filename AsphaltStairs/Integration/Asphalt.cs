using FUtility;
using HarmonyLib;
using System;

namespace AsphaltStairs.Integration
{
    public class Asphalt
    {
        public static float speedModifier = 2f;

        public static void TrySetSpeedModifier()
        {
            var asphaltType = Type.GetType("Asphalt.Mod, Asphalt", false, false);

            if (asphaltType != null)
            {
                var val = asphaltType.GetProperty("Settings").GetValue(null);
                speedModifier = Traverse.Create(val).Property<float>("Speed").Value;
                Log.Info($"Asphalt Tiles mod found. Set speed modifier to {speedModifier}x");

                return;
            }

            Log.Info($"Asphalt Tiles mod not found. Leaving speed multiplier to a default {speedModifier}x.");
        }
    }
}
