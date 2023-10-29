using FUtility;
using Twitchery.Content.Defs.Critters;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class ScheduleManagerPatch
	{
		//[HarmonyPatch(typeof(ScheduleManager), "OnSpawn")]
		public class ScheduleManager_OnSpawn_Patch
		{
			public static void Postfix(ScheduleManager __instance)
			{
				var schedule = __instance.schedules.Find(s => s.name == RegularPipConfig.SCHEDULE_NAME);
				schedule ??= __instance.AddSchedule(Db.Get().ScheduleGroups.allGroups, RegularPipConfig.SCHEDULE_NAME, false);

				foreach (var pip in Mod.regularPips.Items)
				{
					var schedulable = pip.GetComponent<Schedulable>();
					var existingSchedule = __instance.GetSchedule(schedulable);
					if (existingSchedule == null)
					{
						if (schedule == null)
						{
							Log.Warning("Regular pip schedule is not found.");
							schedule = __instance.schedules[0];
						}

						schedule.Assign(schedulable);
					}
				}

				Mod.regularPips.OnAdd += pip => OnAddDupe(__instance, pip);
				Mod.regularPips.OnRemove += pip => OnRemoveDupe(__instance, pip);
			}

			private static void OnAddDupe(ScheduleManager manager, RegularPip pip)
			{
				var schedulable = pip.GetComponent<Schedulable>();

				if (manager.GetSchedule(schedulable) != null)
					return;

				manager.schedules[0].Assign(schedulable);
			}

			private static void OnRemoveDupe(ScheduleManager manager, RegularPip pip)
			{
				var schedulable = pip.GetComponent<Schedulable>();
				manager.GetSchedule(schedulable)?.Unassign(schedulable);
			}
		}
	}
}
