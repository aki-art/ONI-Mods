﻿using HarmonyLib;
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

				if (__instance.TryGetComponent(out BuildingComplete buildingComplete))
				{
					if (buildingComplete.Def.Invincible)
						return;

					if (__instance.TryGetComponent(out Deconstructable _) &&
					__instance.TryGetComponent(out BuildingHP hp))
					{
						var roll = Random.value;

						var chance = __instance.worker.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id);

						if (chance < 0.2f || roll > chance * 0.001f)
							return;

						var tenPercentDamage = Mathf.CeilToInt(hp.MaxHitPoints * 0.05f);

						buildingComplete.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo()
						{
							damage = tenPercentDamage,
							source = (string)BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
							popString = (string)STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.GAMEOBJECTEFFECTS.DAMAGE_POPS.HULK_SMASH
						});

						//buildingComplete.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
						//.notification = this.CreateDamageNotification();
						//this.gameObject.AddOrGet<Notifier>().Add(this.notification);
					}
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
						__result *= TEffects.WORKSPEED_MULTIPLIER;
				}
			}
		}
	}
}
