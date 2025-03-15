using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Pipser : StateMachineComponent<Pipser.StatesInstance>, ISim200ms
	{
		[SerializeField] public float minDormancy;
		[SerializeField] public float maxDormancy;
		[SerializeField] public int minPipPerEruption;
		[SerializeField] public int maxPipPerEruption;
		[SerializeField] public List<Tag> pips;
		[SerializeField] public float timeBetweenPips;
		[SerializeField] public Vector3 spawnOffset;

		[Serialize] public float dormancy;
		[Serialize] public int pipPerEruption;
		[Serialize] public int pipsSpawnedThiscycle;
		[Serialize] public float elapsed;

		public override void OnSpawn()
		{
			if (dormancy == 0)
			{
				dormancy = Random.Range(minDormancy, maxDormancy);
				pipPerEruption = Random.Range(minPipPerEruption, maxPipPerEruption + 1);
				elapsed = Random.Range(0, (maxDormancy + minDormancy) / 2f);
			}

			smi.StartSM();
		}

		public void Sim200ms(float dt)
		{
			elapsed += dt;
		}

		public void SpawnPip()
		{
			pipsSpawnedThiscycle++;
			var tag = pips.GetRandom();
			var pip = FUtility.Utils.Spawn(tag, transform.position + spawnOffset);
			FUtility.Utils.YeetRandomly(pip, true, 1, 4, true);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Pipser, object>.GameInstance
		{
			public StatesInstance(Pipser master) : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, Pipser>
		{
			public State dormant;
			public State idle;
			public State obstructed;
			public State preErupt;
			public State erupting;
			public State pstErupt;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = dormant;

				preErupt
					.PlayAnim("erupt_pre")
					.OnAnimQueueComplete(erupting);

				erupting
					.PlayAnim("spit")
					.UpdateTransition(pstErupt, Spew, UpdateRate.SIM_33ms)
					.ScheduleAction("spit a pip", 0.2f, smi => smi.master.SpawnPip());
			}

			private bool Spew(StatesInstance smi, float dt)
			{
				smi.master.SpawnPip();
				return smi.master.pipsSpawnedThiscycle > smi.master.pipPerEruption;
			}
		}
	}
}

