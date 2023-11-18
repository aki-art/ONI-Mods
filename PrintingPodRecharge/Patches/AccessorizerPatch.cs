using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;

namespace PrintingPodRecharge.Patches
{
	public class AccessorizerPatch
	{
		[HarmonyPatch(typeof(Accessorizer), "OnDeserialized")]
		public class Accessorizer_OnDeserialized_Patch
		{
			[HarmonyPriority(Priority.First)]
			public static void Prefix(Accessorizer __instance, ref List<ResourceRef<Accessory>> ___accessories)
			{
				var nullAccessory = false;
				var identity = __instance.GetComponent<MinionIdentity>();
				foreach (var accessoryRef in __instance.GetAccessories())
				{
					var accessory = accessoryRef.Get();
					if (accessory == null)
					{
						Log.Debug("null accessory for " + identity.nameStringKey);
						nullAccessory = true;
					}
				}

				if (nullAccessory)
				{
					Log.Warning($"Invalid accessories found for {identity.nameStringKey}, resetting to defaults.");

					___accessories.Clear();

					var personality = Db.Get().Personalities.GetPersonalityFromNameStringKey(identity.nameStringKey);
					if (personality == null)
					{
						Log.Warning("No personality with this ID!" +
							"If this is a modded character, please re-enable the mod that added them.");
						return;
					}

					__instance.ApplyMinionPersonality(personality);
				}

				var headshape = __instance.GetAccessory(Db.Get().AccessorySlots.HeadShape);

				if (headshape?.Id == "Root.Accessories.headshape_hulk")
					identity.personalityResourceId = Db.Get().Personalities.TryGet("AKISEXTRATWITCHEVENTS_HULK").Id;
			}
		}
	}
}
