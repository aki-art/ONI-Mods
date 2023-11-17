using Klei.AI;
using System;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Cmps
{
	public class Ghastly : GameStateMachine<Ghastly, Ghastly.Instance, IStateMachineTarget, Ghastly.Def>
	{
		public State idle;
		public PumpkinedStates pumpkined;
		public Signal spiceEatenSignal;

		private const float FADE_DURATION = 3f;

		public class PumpkinedStates : State
		{
			public State day;
			public State night;
			public State nightPre;
			public State nightPst;
#if DEBUG
			public State debug;
#endif
		}

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			idle
				.EnterTransition(pumpkined, HasPumpkinEffect)
				.OnSignal(spiceEatenSignal, pumpkined);

			pumpkined
				.DefaultState(pumpkined.day)
				.EventHandlerTransition(GameHashes.EffectRemoved, idle, IsPumpkinSpice);

			pumpkined.day
				.UpdateTransition(pumpkined.nightPre, (smi, dt) => IsNightTime(smi));

			pumpkined.nightPre
				.EnterTransition(idle, smi => !HasPumpkinEffect(smi))
				.Enter(smi => smi.SetFade(0f))
				.Update((smi, dt) => smi.UpdateFade(smi, dt), UpdateRate.SIM_33ms)
				.ScheduleGoTo(FADE_DURATION, pumpkined.night);

			pumpkined.night
				.Enter(smi => smi.SetFade(1f))
				.Exit(smi => smi.SetFade(0f))
				.ToggleEffect(SPEffects.GHASTLY)
				.ToggleStatusItem(SPStatusItems.ghastlyLitBonus)
				.UpdateTransition(pumpkined.nightPst, (smi, dt) => !IsNightTime(smi));

			pumpkined.nightPst
				.Enter(smi => smi.SetFade(1f))
				.Update((smi, dt) => smi.UpdateFade(smi, -dt), UpdateRate.SIM_33ms)
				.ScheduleGoTo(FADE_DURATION, idle);

#if DEBUG
			pumpkined.debug
				.Enter(smi => smi.SetFade(1f))
				.Exit(smi => smi.SetFade(0f))
				.ToggleEffect(SPEffects.GHASTLY)
				.ToggleStatusItem(SPStatusItems.ghastlyLitBonus);
#endif
		}

		private bool HasPumpkinEffect(Instance smi)
		{
			return smi.effects.HasEffect(SPEffects.PUMPKINED);
		}

		public static void TryApplyHighlight(GameObject go, float value)
		{
			var ghastly = go.GetSMI<Instance>();
			value *= 0.3f;
			if (ghastly != null && ghastly.IsGhastly())
			{
				ghastly.kbac.HighlightColour += new Color(ModAssets.Colors.ghostlyColor.r + value, ModAssets.Colors.ghostlyColor.g + value, ModAssets.Colors.ghostlyColor.b + value);
			}
		}

		private bool IsNightTime(Instance _) => GameClock.Instance.IsNighttime();

		private bool IsPumpkinSpice(Instance _, object data) => data is Effect effect && effect.Id == SPSpices.PUMPKIN_SPICE_ID;

		public class Def : BaseDef
		{
		}

		public new class Instance(IStateMachineTarget master) : GameInstance(master)
		{
			public KBatchedAnimController kbac = master.GetComponent<KBatchedAnimController>();
			public Effects effects = master.GetComponent<Effects>();
			public KSelectable kSelectable = master.GetComponent<KSelectable>();

			private float fade;

			public void OnPumpkinSpiceConsumed()
			{
				smi.sm.spiceEatenSignal.Trigger(smi);
				smi.effects.Add(SPEffects.PUMPKINED, true);
			}

			public bool IsGhastly()
			{
#if DEBUG
				if (smi.IsInsideState(smi.sm.pumpkined.debug))
					return true;
#endif
				return smi.IsInsideState(smi.sm.pumpkined.night);
			}

			public float UpdateEfficiencyBonus(float result, float minimumMultiplier)
			{
				return effects.HasEffect(SPEffects.GHASTLY)
					? Math.Max(result + (Mod.Config.GhastlyWorkBonus / 100f), minimumMultiplier)
					: result;
			}

			public void UpdateFade(Instance _, float dt) => SetFade(fade + (dt / FADE_DURATION));

			public void SetFade(float v)
			{
				if (!Mod.Config.UseGhastlyVisualEffect)
					return;

				fade = Mathf.Clamp01(v);
				kbac.TintColour = Color.LerpUnclamped(Color.white, ModAssets.Colors.transparentTint, fade);
				kbac.HighlightColour = Color.LerpUnclamped(Color.black, ModAssets.Colors.ghostlyColor, fade);
			}
		}
	}
}
