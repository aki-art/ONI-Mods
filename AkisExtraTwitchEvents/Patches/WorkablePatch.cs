using HarmonyLib;
using Klei.AI;
using STRINGS;
using Twitchery.Content;
using UnityEngine;

namespace Twitchery.Patches
{
	public class WorkablePatch
	{
		[HarmonyPatch(typeof(Workable), "WorkTick")]
		public class Workable_WorkTick_Patch
		{
			public static void Postfix(Workable __instance)
			{
				if (__instance.worker == null || !__instance.worker.HasTag(TTags.angry))
					return;

				if (__instance.TryGetComponent(out BuildingComplete buildingComplete) &&
					__instance.TryGetComponent(out BuildingHP hp))
				{
					if (!buildingComplete.Def.Breakable)
						return;

					var roll = Random.value;
					if (roll < 0.1f)
						return;

					var tenPercentDamage = Mathf.CeilToInt(hp.MaxHitPoints * 0.1f);

					buildingComplete.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo()
					{
						damage = tenPercentDamage,
						source = (string)BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
						popString = (string)UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
					});

					//buildingComplete.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
					//.notification = this.CreateDamageNotification();
					//this.gameObject.AddOrGet<Notifier>().Add(this.notification);
				}
			}
		}

		[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier")]
		public class Workable_GetEfficiencyMultiplier_Patch
		{
			[HarmonyPriority(Priority.LowerThanNormal)]
			public static void Postfix(Worker worker, ref float __result)
			{
				if (worker.TryGetComponent(out Effects effects))
				{
					if (effects.HasEffect(TEffects.CAFFEINATED))
					{
						__result *= 1.5f;
					}
				}
			}
		}
	}
}
