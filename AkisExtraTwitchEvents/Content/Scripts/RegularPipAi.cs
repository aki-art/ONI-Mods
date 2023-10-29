namespace Twitchery.Content.Scripts
{
	public class RegularPipAi : GameStateMachine<RegularPipAi, RegularPipAi.Instance>
	{
		public State arriving;
		public State alive;
		public State dead;

		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = root;
			root
				.ToggleStateMachine((smi => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())))
				.Enter(smi => smi.GoTo(smi.HasTag(GameTags.Dead) ? dead : alive));

			alive
				.TagTransition(GameTags.Dead, dead)
				//.ToggleStateMachine((smi => new RationMonitor.Instance(smi.master)))
				//.ToggleStateMachine((smi => new CalorieMonitor.Instance(smi.master)))
				//.ToggleStateMachine((smi => new ThoughtGraph.Instance(smi.master)))
				//.ToggleStateMachine((smi => new SpeechMonitor.Instance(smi.master, new SpeechMonitor.Def())))
				//.ToggleStateMachine((smi => new ReactionMonitor.Instance(smi.master, new ReactionMonitor.Def())))
				//.ToggleStateMachine((smi => new IdleMonitor.Instance(smi.master)))
				//.ToggleThought(Db.Get().Thoughts.Starving)
				.ToggleStateMachine((smi => new MoveToLocationMonitor.Instance(smi.master)));
			//.ToggleStateMachine(smi => new FallMonitor.Instance(smi.master, false));

			dead
				.ToggleBrain("dead")
				.ToggleStateMachine((smi => new FallWhenDeadMonitor.Instance(smi.master)))
				.Enter(smi => smi.RefreshUserMenu())
				.Enter(smi => smi.GetComponent<Storage>().DropAll());
		}


		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				var component = GetComponent<ChoreConsumer>();
				component.AddUrge(Db.Get().Urges.EmoteHighPriority);
				component.AddUrge(Db.Get().Urges.EmoteIdle);
			}

			public void RefreshUserMenu() => Game.Instance.userMenu.Refresh(master.gameObject);
		}
	}
}
