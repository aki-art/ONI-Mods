using ONITwitchLib.Utils;

namespace Twitchery.Content.Scripts
{
	public class AETE_JailMonitor : GameStateMachine<AETE_JailMonitor, AETE_JailMonitor.Instance, IStateMachineTarget, AETE_JailMonitor.Def>
	{
		private static CellOffset[] freedom_offsets =
			[
			CellOffset.up,
			CellOffset.down
		];

		private State free;
		private State jailed;

		public override void InitializeStates(out BaseState default_state)
		{
			serializable = SerializeType.ParamsOnly;
			default_state = free;

			free
				.EnterTransition(jailed, smi => smi.HasTag(TTags.jailed))
				.TagTransition(TTags.jailed, jailed);

			jailed
				.Enter(smi =>
				{
					smi.navigator.CurrentNavType = NavType.Tube;
					smi.allowedToLeave = false;

				})
				.ScheduleAction("allow to leave", smi => 3.0f, smi => smi.allowedToLeave = true)
				.UpdateTransition(free, IsFree)
				.Exit(smi =>
				{
					smi.GetComponent<KPrefabID>().RemoveTag(TTags.jailed);
					smi.navigator.CurrentNavType = NavType.Floor;
				});
		}

		private bool IsFree(Instance smi, float _)
		{
			if (!smi.allowedToLeave)
				return false;

			var cell = Grid.PosToCell(smi);

			// probably temporarily teleported somewhere or stores
			if (!Grid.IsValidCell(cell))
				return false;

			foreach (var offset in freedom_offsets)
			{
				var jailCell = Grid.OffsetCell(cell, offset);

				if (!Grid.IsValidCell(jailCell))
					continue;

				if (GridUtil.IsCellFoundationEmpty(jailCell))
					return true;
			}

			return false;
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Navigator navigator;
			public bool allowedToLeave;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				navigator = GetComponent<Navigator>();
			}
		}
	}
}
