namespace Moonlet.Scripts
{
	public class Moonlet_LightController : GameStateMachine<Moonlet_LightController, Moonlet_LightController.Instance, IStateMachineTarget, Moonlet_LightController.Def>
	{
		public State off;
		public State on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;

			off
				.PlayAnims(smi => [smi.def.offAnimation], smi =>
				{
					Log.Debug("playing off animation with playmode + " + smi.mode.ToString());
					return smi.mode;
				})
				.EventTransition(GameHashes.OperationalChanged, on, (smi => smi.operational.IsOperational));

			on
				.PlayAnims(smi => [smi.def.onAnimation], smi => smi.mode)
				.EventTransition(GameHashes.OperationalChanged, off, (smi => !smi.operational.IsOperational))
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight)
				.Enter(smi => smi.operational.SetActive(true));
		}

		public class Def : BaseDef
		{
			public KAnimHashedString onAnimation = "on";
			public KAnimHashedString offAnimation = "off";
		}

		public new class Instance(IStateMachineTarget master, Def def) : GameInstance(master, def)
		{
			public Operational operational = master.GetComponent<Operational>();
			public KAnim.PlayMode mode = master.GetComponent<KBatchedAnimController>().initialMode;

			public KAnimHashedString GetOnAnim() => def.onAnimation;
		}
	}
}
