using UnityEngine;

namespace Moonlet.Scripts
{
	internal class Moonlet_SingleHarvestable : StateMachineComponent<Moonlet_SingleHarvestable.StatesInstance>
	{
		[MyCmpReq] public Harvestable harvestable;
		[MyCmpReq] public SeedProducer seedProducer;
		[MyCmpReq] public KBatchedAnimController animController;

		[SerializeField] public string deathFx;
		[SerializeField] public string deathAnimation;
		[SerializeField] public string defaultAnimation;
		[SerializeField] public KAnim.PlayMode playMode;

		public Moonlet_SingleHarvestable()
		{
			defaultAnimation = "idle";
		}

		public class StatesInstance(Moonlet_SingleHarvestable smi) : GameStateMachine<States, StatesInstance, Moonlet_SingleHarvestable, object>.GameInstance(smi)
		{
		}

		public class States : GameStateMachine<States, StatesInstance, Moonlet_SingleHarvestable>
		{
			public State seed_grow;
			public AliveStates alive;
			public State dead;

			public class AliveStates : PlantAliveSubState
			{
				public State idle;
				public State harvest;
			}

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = seed_grow;
				serializable = SerializeType.Both_DEPRECATED;

				seed_grow
					.PlayAnim(smi => smi.master.defaultAnimation, KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, alive.idle);

				alive
					.InitializeStates(masterTarget, dead);

				alive.idle
					.PlayAnims(smi => [smi.master.defaultAnimation], smi => smi.master.playMode)
					.EventTransition(GameHashes.Harvest, alive.harvest)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true));

				alive.harvest
					.PlayAnim(smi => smi.master.deathAnimation ?? "harvest")
					.OnAnimQueueComplete(dead)
					.Exit(smi => smi.master.seedProducer.DropSeed());

				dead
					.Enter(Die);
			}

			private static void Die(StatesInstance smi)
			{
				smi.master.PlayDeathEffect();
				smi.master.Trigger((int)GameHashes.Died);
				smi.master.animController.StopAndClear();
				Destroy(smi.master.animController);
				smi.master.DestroySelf();
			}
		}

		private void PlayDeathEffect()
		{
			if (!deathFx.IsNullOrWhiteSpace())
				GameUtil.KInstantiate(
						Assets.GetPrefab(deathFx),
						smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront)
						.SetActive(true);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public void DestroySelf()
		{
			CreatureHelpers.DeselectCreature(gameObject);
			Util.KDestroyGameObject(gameObject);
		}
	}
}
