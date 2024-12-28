using HarmonyLib;
using Klei.AI;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class WaterCoolerPatch
	{
		[HarmonyPatch(typeof(WaterCoolerChore.States), "TriggerDrink")]
		public class WaterCoolerChore_States_TriggerDrink_Patch
		{
			public static void Prefix(WaterCoolerChore.States __instance, WaterCoolerChore.StatesInstance smi)
			{
				var storage = __instance.masterTarget.Get<Storage>(smi);

				if (storage.IsEmpty())
					return;

				var tag = storage.items[0].PrefabID();

				if (TDb.beverages.TryGetValue(tag, out var effect))
					__instance.stateTarget
						.Get<WorkerBase>(smi)
						.GetComponent<Effects>()
						.Add(effect, true);
			}
		}
	}
}
