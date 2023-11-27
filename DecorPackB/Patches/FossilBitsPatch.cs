using DecorPackB.Content.Defs.Items;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class FossilBitsPatch
	{
		[HarmonyPatch(typeof(FossilBits), "DropLoot")]
		public class FossilBits_DropLoot_Patch
		{
			public static void Postfix(FossilBits __instance)
			{
				if (__instance.TryGetComponent(out PrimaryElement primaryElement))
				{
					var nodule = Utils.Spawn(FossilNoduleConfig.ID, __instance.gameObject);
					if (nodule.TryGetComponent(out PrimaryElement primaryElement2))
					{
						primaryElement2.SetTemperature(primaryElement.Temperature);
						primaryElement2.AddDisease(primaryElement.DiseaseIdx, (int)(primaryElement2.DiseaseCount * 0.1f), "Spawn");
					}
				}
			}
		}
	}
}
