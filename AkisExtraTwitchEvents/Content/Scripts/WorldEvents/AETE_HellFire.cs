using ONITwitchLib;
using ONITwitchLib.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts.WorldEvents
{
	public class AETE_HellFire : AETE_WorldEvent, ISim33ms
	{
		[SerializeField] public float lavaRadius;

		[SerializeField] public float density;

		private int _myWorldIdx;
		[SerializeField] private LiquidRainSpawner rain;

		public override void Begin()
		{
			var go = new GameObject("lava cloud spawner");
			var cursor = go.AddComponent<WarningCursor>();

			cursor.OnTimerDoneFn += BeginLavaRain;
			cursor.startDelaySeconds = 1.5f;
			cursor.endDelaySeconds = 0.1f;
			cursor.timer = 6f;
			cursor.disallowRocketInteriors = false;
			cursor.disallowProtectedCells = false;
			cursor.overTimer = 120f;
			cursor.animationFile = "aete_warning_kanim";

			rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (9999, 9999);
			rain.durationInSeconds = 9999;
			rain.dropletMassKg = 0.1f;
			rain.spawnRadius = lavaRadius;
			rain.updatePosition = true;

			go.SetActive(true);

			cursor.StartTimer();

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.PLACEGEYSER.TOAST,
				STRINGS.AETE_EVENTS.PLACEGEYSER.DESC);



			rain.AddElement(SimHashes.Magma, 1f, GameUtil.GetTemperatureConvertedToKelvin(2500, GameUtil.TemperatureUnit.Celsius));


			// visualizer
			base.Begin();

			Log.Debug($"toggled overlay for {this.GetMyWorldId()} ");
			AkisTwitchEvents.Instance.ToggleOverlay(this.GetMyWorldId(), OverlayRenderer.HELLFIRE, true, false);
		}

		private void BeginLavaRain(Transform _)
		{
			rain.StartRaining();
		}

		protected override void Initialize()
		{
			base.Initialize();

			var world = this.GetMyWorld();
			_myWorldIdx = world.id;

			if (Stage == WorldEventStage.Active)
				AkisTwitchEvents.Instance.ToggleOverlay(_myWorldIdx, OverlayRenderer.HELLFIRE, true, true);
		}

		public override void End()
		{
			base.End();
			// remove vis
			AkisTwitchEvents.Instance.ToggleOverlay(_myWorldIdx, OverlayRenderer.HELLFIRE, false, false);
			rain.StopRaining(true);
			Util.KDestroyGameObject(gameObject);
		}


		public void Sim33ms(float dt)
		{
			elapsedTime += dt;

			if (Stage == WorldEventStage.Active)
			{
				if (!rain.IsNullOrDestroyed())
				{
					var originCell = PosUtil.ClampedMouseCell();
					rain.transform.position = Grid.CellToPos(originCell);
				}
				else
					Log.Debug("rain is null");

				if (elapsedTime > durationInSeconds)
					End();
			}
		}
	}
}