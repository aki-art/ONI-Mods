using FUtility;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class DramaticTentaclesEntrance : GameStateMachine<DramaticTentaclesEntrance, DramaticTentaclesEntrance.Instance>
	{
		public static Instance PlayFx(GameObject gameObject, Vector3 offset)
		{
			var instance = new Instance(gameObject.GetComponent<KMonoBehaviour>(), offset);
			instance.StartSM();
			return instance;
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
			{
				FUtility.Log.Debuglog("spawned tentacles");
				var position = (master.gameObject.transform.GetPosition() + offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.Ore) };
				var kbac = FXHelpers.CreateEffect("aete_tentacleportal_kanim", position, master.gameObject.transform, true);

				FUtility.Log.Assert("kbac", kbac);

				sm.fx.Set(kbac.gameObject, smi);
			}

			public void DestroyFX()
			{
				FUtility.Log.Debuglog("destroy fx");
				Util.KDestroyGameObject(sm.fx.Get(smi));
			}

			public Reactable CreateReactable()
			{
				return new EmoteReactable(master.gameObject, "AkisExtraTwitchEvents_Shock", Db.Get().ChoreTypes.Emote)
					.SetEmote(Db.Get().Emotes.Minion.Wave_Shy);
			}

			public void SpawnTentacles()
			{
				for (int i = 0; i < 3; i++)
				{
					var kbac = FXHelpers.CreateEffect("aete_tentacleportal_kanim", transform.position, master.gameObject.transform, true);

					var speed = Random.Range(0.66f, 1f);
					var timeOffset = Random.Range(0, 2f);
					kbac.Stop();
					kbac.destroyOnAnimComplete = false;

					GameScheduler.Instance.Schedule("play tentacle", timeOffset, _ =>
					{
						kbac.destroyOnAnimComplete = true;
						kbac.Play("tentacle_arm", KAnim.PlayMode.Once, speed);
					});

					var angle = Random.Range(0, 360f);
					var rot = Quaternion.AngleAxis(angle, Vector3.forward);
					kbac.Offset = rot * (Vector3.up * 0.3f);
					kbac.Offset += Vector3.up;
					kbac.FlipX = Random.value < 0.5f;
					kbac.Rotation = angle;
					kbac.destroyOnAnimComplete = true;
					kbac.animScale *= Random.Range(0.8f, 1f);
				}
			}
		}

		public TargetParameter fx;

		public State appear;
		public State idle;
		public State closing;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = appear;
			Target(fx);

			root
				.ToggleReactable(smi => smi.CreateReactable());

			appear
				.QueueAnim("vortex_appear")
				.OnAnimQueueComplete(idle);

			idle
				.Enter(smi => smi.SpawnTentacles())
				.PlayAnim("vortex_loop", KAnim.PlayMode.Loop)
				.ScheduleGoTo(3f, closing);

			closing
				.QueueAnim("vortex_close")
				.OnAnimQueueComplete(null)
				.Exit(smi => smi.DestroyFX());
		}
	}
}
