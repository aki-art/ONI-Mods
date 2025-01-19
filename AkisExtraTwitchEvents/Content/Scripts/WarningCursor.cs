using System;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class WarningCursor : KMonoBehaviour
	{
		public Action<Transform> OnTimerDoneFn;

		[SerializeField] public float timer;
		[SerializeField] public float startDelaySeconds;
		[SerializeField] public float endDelaySeconds;

		private float timerSmall;
		private float elapsed;
		private float elapsedSmall;
		private KBatchedAnimController kbac;
		private bool isRunning;

		public override void OnSpawn()
		{
			base.OnSpawn();
			timerSmall = startDelaySeconds;
			kbac = FXHelpers.CreateEffect("aete_warning_kanim", transform.position, transform.parent, false, Grid.SceneLayer.FXFront2);
			kbac.destroyOnAnimComplete = false;
			kbac.Play("on");
			kbac.gameObject.SetActive(true);
		}

		public void StartTimer() => isRunning = true;

		void Update()
		{
			if (!isRunning || Game.Instance.IsPaused)
				return;

			if (kbac != null)
				kbac.transform.position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());

			elapsed += Time.deltaTime;
			if (elapsed > timer)
			{
				OnTimerDoneFn?.Invoke(transform);
				Util.KDestroyGameObject(kbac.gameObject);
				Util.KDestroyGameObject(gameObject);

				return;
			}

			elapsedSmall += Time.deltaTime;

			if (elapsedSmall > timerSmall)
			{
				var elapsed01 = Mathf.Clamp01(elapsed / timer);
				timerSmall = Mathf.Lerp(startDelaySeconds, endDelaySeconds, elapsed01);
				elapsedSmall = 0;

				if (kbac != null)
				{
					//kbac.SetVisiblity(kbac.isVisible);

					if (kbac.isVisible)
						AudioUtil.PlaySound(ModAssets.Sounds.WARNING, ModAssets.GetSFXVolume() * 0.8f);
				}
			}
		}
	}
}
