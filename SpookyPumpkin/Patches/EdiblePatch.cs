using HarmonyLib;
using SpookyPumpkinSO.Content;
using SpookyPumpkinSO.Content.Cmps;
using System.Collections.Generic;
using System.Linq;

namespace SpookyPumpkinSO.Patches
{
	public class EdiblePatch
	{
		[HarmonyPatch(typeof(Edible), "OnPrefabInit")]
		public class Edible_OnPrefabInit_Patch
		{
			public static void Postfix(Edible __instance) => __instance.gameObject.AddOrGet<SpiceRestorer>();
		}

		[HarmonyPatch(typeof(Edible), "StopConsuming")]
		public class Edible_StopConsuming_Patch
		{
			public static void Postfix(Worker worker, List<SpiceInstance> ___spices)
			{
				if (___spices.Any(spice => spice.Id == SPSpices.PumpkinSpice.Id))
				{
					var ghastly2 = worker.GetSMI<Ghastly.Instance>();

					if (ghastly2 != null)
						ghastly2.OnPumpkinSpiceConsumed();
				}
			}
		}
	}
}
