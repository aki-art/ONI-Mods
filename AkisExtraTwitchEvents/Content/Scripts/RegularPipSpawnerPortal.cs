using Twitchery.Content.Defs.Critters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts
{
	public class RegularPipSpawnerPortal : StateMachineComponent<RegularPipSpawnerPortal.SMInstance>, ISidescreenButtonControl
	{
		[SerializeField] public float portalOpenTimeSeconds;
		[SerializeField] public float tentacleSpawnChance;

		public string SidescreenButtonText => "Inspect closer";

		public string SidescreenButtonTooltip => "???";

		public int ButtonSideScreenSortOrder() => 0;

		public int HorizontalGroupID() => -1;

		public void OnSidescreenButtonPressed()
		{
			CameraController.Instance.SetTargetPos(transform.position, 6f, false);

			var kSelectable = GetComponent<KSelectable>();
			kSelectable.Unselect();
			kSelectable.selectable = false;

			smi.sm.openPortal.Trigger(smi);
		}

		public override void OnSpawn()
		{
			var z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
			transform.position = transform.position with { z = z };

			smi.StartSM();
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

		public bool SidescreenButtonInteractable() => true;

		public bool SidescreenEnabled() => smi.IsInsideState(smi.sm.anomalyIdle);

		public class States : GameStateMachine<States, SMInstance, RegularPipSpawnerPortal>
		{
			public State anomalyAppear;
			public State anomalyIdle;
			public State open;
			public State summoningPip;
			public State close;

			public Signal openPortal;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = anomalyAppear;

				anomalyAppear
					.PlayAnim("anomaly_pre")
					.OnAnimQueueComplete(anomalyIdle);

				anomalyIdle
					.PlayAnim("anomaly_loop", KAnim.PlayMode.Loop)
					.OnSignal(openPortal, open);

				open
					.PlayAnim("anomaly_pst")
					.Update(TrySpawnTentacle)
					.OnAnimQueueComplete(summoningPip);

				summoningPip
					.PlayAnim("vortex_loop")
					.Enter(CreateFakePip)
					.ScheduleGoTo(smi => smi.master.portalOpenTimeSeconds, close);

				close
					.PlayAnim("vortex_close")
					.OnAnimQueueComplete(null)
					.Exit(smi => Util.KDestroyGameObject(smi.master.gameObject));
			}

			private void TrySpawnTentacle(SMInstance smi, float dt)
			{
				if (Random.value < smi.master.tentacleSpawnChance)
					SpawnTentacle(smi.transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.Ore) });
			}

			private void SpawnTentacle(Vector3 position)
			{
				var kbac = FXHelpers.CreateEffect("aete_tentacleportal_kanim", position);

				var speed = Random.Range(0.66f, 1f);

				kbac.destroyOnAnimComplete = true;
				kbac.Play("tentacle_arm", KAnim.PlayMode.Once, speed);

				var angle = Random.Range(0, 360f);
				var rot = Quaternion.AngleAxis(angle, Vector3.forward);
				kbac.Offset = rot * (Vector3.up * 0.3f);
				kbac.FlipX = Random.value < 0.5f;
				kbac.Rotation = angle;
				kbac.destroyOnAnimComplete = true;
				kbac.animScale *= Random.Range(0.8f, 1f);
			}

			private void CreateFakePip(SMInstance smi)
			{
				var position = (smi.transform.position + Vector3.down) with { z = Grid.GetLayerZ(Grid.SceneLayer.Move) };
				var pip = FXHelpers.CreateEffect("squirrel_kanim", position);
				pip.Play("grooming_pst");
				pip.destroyOnAnimComplete = true;
				pip.onAnimComplete += _ => CreateActualPip(position);
			}

			private void CreateActualPip(Vector3 position)
			{
				var pip = FUtility.Utils.Spawn(RegularPipConfig.ID, position, Grid.SceneLayer.Move);
				pip.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				pip.transform.SetPosition(position);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, RegularPipSpawnerPortal, object>.GameInstance
		{
			public SMInstance(RegularPipSpawnerPortal master) : base(master) { }
		}
	}
}
