using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;

namespace SpookyPumpkinSO.Patches
{
	public class WorkablePatch
	{
		[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier")]
		public class Workable_GetEfficiencyMultiplier_Patch
		{
			public static void Postfix(Worker worker, float ___minimumAttributeMultiplier, ref float __result)
			{
				var smi = worker.GetSMI<Ghastly.Instance>();
				if (smi != null)
				{
					__result = smi.UpdateEfficiencyBonus(__result, ___minimumAttributeMultiplier);
				}
			}
		}
	}
}
