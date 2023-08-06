using HarmonyLib;
using Klei.AI;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class WaterCoolerPatch
	{
		[HarmonyPatch(typeof(WaterCoolerChore.States), "Drink")]
		public class WaterCoolerChore_States_Drink_Patch
		{
			public static void Prefix(WaterCoolerChore.States __instance, WaterCoolerChore.StatesInstance smi)
			{
				var storage = __instance.masterTarget.Get<Storage>(smi);

				if (storage.IsEmpty())
					return;

				Tag tag = storage.items[0].PrefabID();

				if (TDb.beverages.TryGetValue(tag, out var effect))
					__instance.stateTarget
						.Get<Worker>(smi)
						.GetComponent<Effects>()
						.Add(effect, true);
			}
		}
	}
}
