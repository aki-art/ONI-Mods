using Twitchery.Content.Defs.Critters;

namespace Twitchery.Patches
{
	public class ScheduleMinionWidgetPatch
	{
		//[HarmonyPatch(typeof(ScheduleMinionWidget), "Setup")]
		public class ScheduleMinionWidget_Setup_Patch
		{
			public static bool Prefix(Schedulable schedulable, ScheduleMinionWidget __instance)
			{
				if (schedulable.IsPrefabID(RegularPipConfig.ID))
				{
					__instance.label.text = schedulable.GetProperName();
					return false;
				}

				return true;
			}
		}
	}
}
