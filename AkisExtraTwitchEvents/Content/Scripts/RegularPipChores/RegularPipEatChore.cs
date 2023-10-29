using FUtility;
using Twitchery.Content.Defs.Critters;

namespace Twitchery.Content.Scripts.RegularPipChores
{
	public class RegularPipEatChore : Chore<RegularPipEatChore.StatesInstance>
	{
		private Pickupable pickupable;
		private RegularPipEdible pipEdible;

		public static readonly Precondition IsPipCondition = new()
		{
			id = "AkisExtraTwitchEvents_IsAPip",
			description = "Not a Pip",
			fn = IsRegularPip
		};

		public static readonly Precondition CanEatCondition = new()
		{
			id = "AkisExtraTwitchEvents_CanPipEat",
			description = "Not hungry",
			fn = IsHungry
		};

		private static bool IsRegularPip(ref Precondition.Context context, object data) => context.consumerState.prefabid.PrefabTag == RegularPipConfig.ID;

		private static bool IsHungry(ref Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.Hungry);

		public RegularPipEatChore(RegularPipEdible master) : base(
			Db.Get().ChoreTypes.TakeMedicine,//TChoreTypes.pipEat, 
			master,
			null,
			false,
			master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
		{
			pipEdible = master;
			pickupable = pipEdible.GetComponent<Pickupable>();
			smi = new StatesInstance(this);
			AddPrecondition(IsPipCondition);
			AddPrecondition(CanEatCondition);
			AddPrecondition(ChorePreconditions.instance.CanPickup, pickupable);
		}

		public override void Begin(Precondition.Context context)
		{
			smi.sm.source.Set(pickupable.gameObject, smi, false);

			//Attributes attributes = context.consumerState.gameObject.GetAttributes();
			//var calories = Db.Get().Amounts.Calories;

			//var amount = attributes.GetValue(calories.Id) / attributes.GetValue(calories.maxAttribute.Id);
			smi.sm.requestedpillcount.Set(1f, smi);
			smi.sm.eater.Set(context.consumerState.gameObject, smi, false);
			base.Begin(context);
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
					.OnAnimQueueComplete(null)
					.Exit(smi =>
					{
						Log.Debuglog("ate something");

						if (chunk == null || eater == null)
							return;

						var workable = chunk.Get<RegularPipEdible>(smi);
						var pip = eater.Get(smi);

						if (workable == null || pip == null)
							return;

						Log.Debuglog("\tate: " + workable.PrefabID());
						Log.Debuglog("\tamount: " + smi.sm.actualpillcount.Get(smi));
						Log.Debuglog("\ttotal: " + workable.kcalPerKg * smi.sm.actualpillcount.Get(smi));
						pip.Trigger((int)GameHashes.CaloriesConsumed, new CreatureCalorieMonitor.CaloriesConsumedEvent()
						{
							tag = workable.PrefabID(),
							calories = workable.kcalPerKg * smi.sm.actualpillcount.Get(smi)
						});
					});
			}

			private bool IsEdibleByPip(StatesInstance smi) => chunk.Get<RegularPipEdible>(smi) != null;
		}

	}
}
