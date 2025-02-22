using Klei.AI;

namespace Twitchery.Content.Scripts
{

	public class AETE_SolarStormBionicReactionMonitor : GameStateMachine<AETE_SolarStormBionicReactionMonitor, AETE_SolarStormBionicReactionMonitor.Instance, IStateMachineTarget, AETE_SolarStormBionicReactionMonitor.Def>
	{
		private State safe;
		private State suffering;

		public override void InitializeStates(out BaseState default_state)
		{
			serializable = SerializeType.ParamsOnly;
			default_state = safe;

			safe
				.Enter(smi => smi.effects.Remove(TEffects.BIONIC_SOLAR_ZAP)) // just to make sure, sometimes it gets stuck on reload it seems
				.EventTransition(ModEvents.SolarStormBegan, suffering)
				.EnterTransition(suffering, IsSuffering);

			suffering
				.EnterTransition(safe, Not(IsSuffering))
				.EventTransition(ModEvents.SolarStormEnded, safe)
				.ToggleEffect(TEffects.BIONIC_SOLAR_ZAP)
				.ToggleReactable(ZapReactable);

		}

		private static Reactable ZapReactable(Instance smi) => smi.GetZapReactable();

		private static bool IsSuffering(Instance smi) => AkisTwitchEvents.Instance.IsSolarStormActive() && AkisTwitchEvents.MaxDanger > ONITwitchLib.Danger.Small;

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Effects effects;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				this.effects = this.GetComponent<Effects>();
			}

			public Reactable GetZapReactable()
			{
				var zapReactable = new SelfEmoteReactable(
					this.master.gameObject,
					(HashedString)Db.Get().Emotes.Minion.WaterDamage.Id,
					Db.Get().ChoreTypes.WaterDamageZap,
					localCooldown: BionicWaterDamageMonitor.Def.ZapInterval);

				zapReactable.SetEmote(Db.Get().Emotes.Minion.WaterDamage);
				zapReactable.preventChoreInterruption = true;

				return zapReactable;
			}
		}
	}
}
