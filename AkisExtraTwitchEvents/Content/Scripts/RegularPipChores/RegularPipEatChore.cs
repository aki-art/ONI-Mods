#if WIP_EVENTS
using Twitchery.Content.Defs.Critters;

namespace Twitchery.Content.Scripts.RegularPipChores
{
	public class RegularPipEatChore : Chore<RegularPipEatChore.StatesInstance>
	{
		private Pickupable pickupable;
		private RegularPipEdible pipEdible;

		public static readonly Precondition CanEatCondition = new()
		{
			id = "AkisExtraTwitchEvents_CanPipEat",
			description = "Can eat",
			fn = CanEat
		};

		private static bool CanEat(ref Precondition.Context context, object data)
		{
			return context.consumerState.prefabid.PrefabTag == RegularPipConfig.ID
				&& context.consumerState.prefabid.HasTag(GameTags.Creatures.Hungry);
		}

		public RegularPipEatChore(RegularPipEdible master) : base(Db.Get().ChoreTypes.Eat, master, null, false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
		{
			pipEdible = master;
			pickupable = pipEdible.GetComponent<Pickupable>();
			smi = new StatesInstance(this);
			AddPrecondition(ChorePreconditions.instance.CanPickup, pickupable);
			AddPrecondition(CanEatCondition, this);
			AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
		}

		public override void Begin(Precondition.Context context)
		{
			smi.sm.source.Set(pickupable.gameObject, smi, false);
			double num = (double)smi.sm.requestedpillcount.Set(1f, smi);
			smi.sm.eater.Set(context.consumerState.gameObject, smi, false);
			base.Begin(context);
			new RegularPipEatChore(pipEdible);
		}


		public class StatesInstance : GameStateMachine<States, StatesInstance, RegularPipEatChore, object>.GameInstance
		{
			public StatesInstance(RegularPipEatChore master) : base(master)
			{
			}
		}

		public class States :
		  GameStateMachine<States, StatesInstance, RegularPipEatChore>
		{
			public TargetParameter eater;
			public TargetParameter source;
			public TargetParameter chunk;
			public FloatParameter requestedpillcount;
			public FloatParameter actualpillcount;
			public FetchSubState fetch;
			public State takemedicine;
			public State postEating;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = fetch;

				Target(eater);

				fetch
					.InitializeStates(
						eater,
						source,
						chunk,
						requestedpillcount,
						actualpillcount,
						takemedicine);

				takemedicine
					//.ToggleAnims("anim_eat_floor_kanim")
					.PlayAnim("eat_pre")
					.QueueAnim("eat_loop", true)
					.ToggleTag(TTags.eating)
					.ToggleWork("AETE Eat", smi =>
					{
						var workable = chunk.Get<RegularPipEdible>(smi);
						eater
							.Get<Worker>(smi)
							.StartWork(new Worker.StartWorkInfo(workable));
					}, IsEdibleByPip, postEating, postEating);

				postEating
					.PlayAnim("eat_pst")
					.OnAnimQueueComplete(null);
			}

			private bool IsEdibleByPip(StatesInstance smi) => chunk.Get<RegularPipEdible>(smi) != null;
		}

	}
}
#endif