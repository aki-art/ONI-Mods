using HarmonyLib;
using Moonlet.Scripts;

namespace Moonlet.Patches
{
	public class SteppedInMonitorPatch
	{
		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.GetCarpetFeet), typeof(SteppedInMonitor.Instance))]
		public class SteppedInMonitor_GetCarpetFeet_Patch
		{
			public static void Postfix(SteppedInMonitor.Instance smi)
			{
				Log.Debug("SteppedInMonitor.GetCarpetFeet");
				Log.Debug(0);
				if (smi == null || smi.effects == null || Moonlet_Mod.stepOnEffects == null)
					return;

				Log.Debug(1);
				var cell = Grid.CellBelow(Grid.PosToCell(smi));

				if (!Grid.IsValidCell(cell))
					return;

				var element = Grid.Element[cell].id;


				Log.Debug(2);
				if (Moonlet_Mod.stepOnEffects.TryGetValue(element, out var effects))
				{
					Log.Debug(3);
					if (effects == null || effects.WalkedOn == null)
						return;

					if (effects.WalkedOn.RemoveEffects != null)
					{
						foreach (var effect in effects.WalkedOn.RemoveEffects)
							smi.effects.Remove(effect);
					}

					Log.Debug(4);
					if (!effects.WalkedOn.Id.IsNullOrWhiteSpace())
					{
						smi.effects.Add(effects.WalkedOn.Id, true);
					}
					Log.Debug(5);
				}

				Log.Debug(6);
			}
		}

		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.GetSoaked), typeof(SteppedInMonitor.Instance))]
		public class SteppedInMonitor_GetSoaked_Patch
		{
			public static void Postfix(SteppedInMonitor.Instance smi)
			{
				if (smi.effects == null) return;
				if (Moonlet_Mod.stepOnEffects == null)
					return;
				var cell = Grid.PosToCell(smi);

				if (!Grid.IsValidCell(cell))
					return;

				var element = Grid.Element[cell].id;

				if (Moonlet_Mod.stepOnEffects.TryGetValue(element, out var effects))
				{
					if (effects == null)
						return;

					if (effects.SubmergedIn == null)
						return;

					if (effects.SubmergedIn.RemoveEffects != null)
					{
						foreach (var effect in effects.SubmergedIn.RemoveEffects)
							smi.effects.Remove(effect);
					}

					if (effects.SubmergedIn.Id == null)
						return;

					if (smi.effects.HasImmunityTo(ModDb.wet))
						return;

					if (effects.SteppedIn != null)
						smi.effects.Remove(effects.SteppedIn.Id);

					smi.effects.Add(effects.SubmergedIn.Id, true);
				}
			}
		}

		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.GetWetFeet), typeof(SteppedInMonitor.Instance))]
		public class SteppedInMonitor_GetWetFeet_Patch
		{
			// TODO: only patch if Moonlet_Mod.stepOnEffects has anything
			public static void Postfix(SteppedInMonitor.Instance smi)
			{
				if (smi.effects == null) return;

				if (Moonlet_Mod.stepOnEffects == null || smi == null)
					return;

				var cell = Grid.PosToCell(smi);

				if (!Grid.IsValidCell(cell))
					return;

				var element = Grid.Element[cell].id;

				if (Moonlet_Mod.stepOnEffects.TryGetValue(element, out var effects))
				{
					if (effects == null) return;

					if (effects.SteppedIn == null || smi.effects == null)
						return;

					if (effects.SteppedIn.RemoveEffects != null)
					{
						foreach (var effect in effects.SteppedIn.RemoveEffects)
							smi.effects.Remove(effect);
					}

					/*					for (int i = smi.effects.effects.Count - 1; i >= 0; i--)
										{
											Klei.AI.EffectInstance effectInstance = smi.effects.effects[i];

											var id = effectInstance.effect.Id;
											if (ModDb.effectTags.TryGetValue(id, out var tags))
											{
												if (tags.Contains(ModTags.EffectTags.WetFeet))
													smi.effects.Remove(effectInstance.effect);
											}
										}*/

					if (effects.SteppedIn.Id == null)
						return;

					if (smi.effects.HasImmunityTo(ModDb.wetFeet))
						return;

					if (effects.SubmergedIn != null && !smi.effects.HasEffect(effects.SubmergedIn.Id))
						smi.effects.Add(effects.SteppedIn.Id, true);
				}
			}
		}
	}
}
