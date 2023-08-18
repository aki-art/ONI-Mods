using HarmonyLib;
using Klei.AI;
using Moonlet.Loaders;

namespace Moonlet.Patches
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

				if (SharedElementsLoader.beverages.TryGetValue(tag, out var effects))
				{
					foreach (var effect in effects)
					{
						__instance.stateTarget
							.Get<Worker>(smi)
							.GetComponent<Effects>()
							.Add(effect, true);
					}
				}
			}
		}
	}
}
