using HarmonyLib;
using SpookyPumpkinSO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpookyPumpkinSO.Patches
{
    internal class EdiblePatch
    {

        [HarmonyPatch(typeof(Edible), "AddOnConsumeEffects")]
        public class Edible_AddOnConsumeEffects_Patch
        {
            public static void Postfix(Worker worker, List<SpiceInstance> ___spices)
            {
                if(___spices.Any(spice => spice.Id == SPSpices.PumpkinSpice.Id))
                {

                }
            }
        }
    }
}
