using HarmonyLib;

namespace Beached.Patches
{
    internal class GasBreatherFromWorldProviderPatch
    {
        [HarmonyPatch(typeof(GasBreatherFromWorldProvider), "OnSimConsume")]
        public class GasBreatherFromWorldProvider_OnSimConsume_Patch
        {
            public static void Postfix(Sim.MassConsumedCallback mass_cb_info, OxygenBreather ___oxygenBreather)
            {
                if (ElementLoader.elements[mass_cb_info.elemIdx].id == Elements.SaltyOxygen)
                {
                    ___oxygenBreather.Trigger((int)ModHashes.GreatAirQuality, mass_cb_info);
                }
            }
        }
    }
}
