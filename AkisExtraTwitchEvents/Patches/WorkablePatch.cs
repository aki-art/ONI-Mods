using FUtility;
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

				if (__instance.TryGetComponent(out BuildingComplete buildingComplete))
				{
					if (buildingComplete.Def.Invincible)
						return;

					if (__instance.TryGetComponent(out Deconstructable _) &&
					__instance.TryGetComponent(out BuildingHP hp))
					{
						var roll = Random.value;

						if (roll > 0.0005f)
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
			public static void Prefix(Workable __instance, Worker worker)
			{
				Log.Assert("worker", worker);

				if (__instance.attributeConverter != null)
				{
					Log.Debuglog("attribute id: " + __instance.attributeConverter.Id);
					var workerConverters = worker.GetComponent<AttributeConverters>();
					Log.Assert("worker converters", workerConverters);
					AttributeConverterInstance converter = workerConverters.GetConverter(__instance.attributeConverter.Id);
					Log.Assert("converter", converter);
					
				}
				else
				{
					Log.Debuglog("attributeConverter is null");
				}
			}

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
