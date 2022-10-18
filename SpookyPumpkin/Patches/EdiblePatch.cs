using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;
using SpookyPumpkinSO.Content.Cmps;
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
                    Log.Debuglog("ate pumpkin spiced food");
                    var ghastly2 = worker.GetSMI<Ghastly2.Instance>();
                    if(ghastly2 != null)
                    {
                        ghastly2.OnPumpkinSpiceConsumed();
                    }
                    else
                    {
                        Log.Debuglog("NO SMI");
                    }
                }
            }
        }
    }
}
