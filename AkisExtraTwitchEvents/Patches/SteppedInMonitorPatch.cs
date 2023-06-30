using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class SteppedInMonitorPatch
	{

		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.GetWetFeet), typeof(SteppedInMonitor.Instance))]
		public class SteppedInMonitor_GetWetFeet_Patch
		{
			public static void Postfix(SteppedInMonitor.Instance smi)
			{
				var cell = Grid.PosToCell(smi);
				if (Grid.IsValidCell(cell) && Grid.Element[cell].id == Elements.PinkSlime)
				{
					if (!smi.effects.HasImmunityTo(TDb.wetFeet)
						&& !smi.effects.HasEffect(TEffects.SOAKEDINSLIME))
						smi.effects.Add(TEffects.STEPPEDINSLIME, true);
				}
			}
		}

		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.GetSoaked), typeof(SteppedInMonitor.Instance))]
		public class SteppedInMonitor_GetSoaked_Patch
		{
			public static void Postfix(SteppedInMonitor.Instance smi)
			{
				var cell = Grid.CellAbove(Grid.PosToCell(smi));

				if (Grid.IsValidCell(cell) && Grid.Element[cell].id == Elements.PinkSlime)
				{
					if (!smi.effects.HasImmunityTo(TDb.wet))
					{
						smi.effects.Remove(TEffects.STEPPEDINSLIME);
						smi.effects.Add(TEffects.SOAKEDINSLIME, true);
					}
				}
			}
		}
	}
}
