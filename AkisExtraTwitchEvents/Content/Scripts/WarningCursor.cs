using ONITwitchLib.Utils;
using System;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class WarningCursor : KMonoBehaviour
	{
		public Action<Transform> OnTimerDoneFn;

		[SerializeField] public float timer;
		[Tooltip("In deconds, used for extra duration if the protected cells are checked for")]
		[SerializeField] public float overTimer;
		[SerializeField] public float startDelaySeconds;
		[SerializeField] public float endDelaySeconds;
		[SerializeField] public bool disallowProtectedCells;
		[SerializeField] public bool disallowRocketInteriors;
		[SerializeField] public string animationFile;
		[SerializeField] public bool snapToTile;

		private float timerSmall;
		private static readonly float messageTimer = 3f;
		private float elapsed;
		private float elapsedSmall;
		private float elapsedMessage;
		private KBatchedAnimController kbac;
		private bool isRunning;

		public override void OnSpawn()
		{
			base.OnSpawn();
			timerSmall = startDelaySeconds;
			kbac = FXHelpers.CreateEffect(animationFile, transform.position, transform.parent, false, Grid.SceneLayer.FXFront2);
			kbac.destroyOnAnimComplete = false;
			kbac.Play("on", KAnim.PlayMode.Loop);
			kbac.isMovable = true;
			kbac.transform.parent = transform.parent;
			kbac.gameObject.SetActive(true);
		}

		public void StartTimer()
		{
			elapsed = 0;
			elapsedMessage = 999;
			isRunning = true;
		}

		void Update()
		{
			if (!isRunning || Game.Instance.IsPaused)
				return;

			if (kbac != null)
			{
				var cursorPos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
				if (snapToTile)
				{
					/*float increment = 0.5f;
										cursorPos = new Vector3(
											Mathf.Round(cursorPos.x / increment) * increment,
											Mathf.Round(cursorPos.y / increment) * increment,
											Grid.GetLayerZ(Grid.SceneLayer.FXFront2));*/
					cursorPos = new Vector3(
					(int)cursorPos.x + 0.5f,
					(int)cursorPos.y + 0.5f,
					Grid.GetLayerZ(Grid.SceneLayer.FXFront2));
				}
				else
					cursorPos = cursorPos with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };

				kbac.transform.position = cursorPos;
				kbac.SetDirty();
				kbac.UpdateAnim(0);
			}

			elapsed += Time.deltaTime;
			var isInOverTime = false;

			if (elapsed > timer)
			{
				var success = true;
				if (disallowProtectedCells)
				{
					var cell = Grid.PosToCell(PosUtil.ClampedMouseWorldPos());
					if (AGridUtil.protectedCells.Contains(cell))
					{
						Log.Debug("protected cell");
						success = false;
					}
				}

				if (success && disallowRocketInteriors)
				{
					if (ClusterManager.Instance.activeWorld.IsModuleInterior)
					{
						Log.Debug("protected interior");
						success = false;
					}
				}

				if (success)
				{
					OnTimerDoneFn?.Invoke(transform);
					Util.KDestroyGameObject(kbac.gameObject);
					Util.KDestroyGameObject(gameObject);

					return;
				}
				else
				{
					isInOverTime = true;
				}
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
					kbac.PlaySpeedMultiplier = timerSmall;
					if (kbac.isVisible)
					{
						if (!isInOverTime)
							AudioUtil.PlaySound(ModAssets.Sounds.WARNING, ModAssets.GetSFXVolume() * 0.8f);
					}
				}

				if (isInOverTime)
				{
					if (elapsed > timer + overTimer)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, $"<color=#FF4444><size=120%>{STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.COULD_NOT_PLACE}</size></color>", kbac.transform, 5f);

						Util.KDestroyGameObject(kbac.gameObject);
						Util.KDestroyGameObject(gameObject);

						return;
					}
					elapsedMessage += Time.deltaTime;
					if (elapsedMessage > messageTimer)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, $"<color=#FF4444><size=120%>{STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.PROTECTED}</size></color>", kbac.transform);
						AudioUtil.PlaySound(ModAssets.Sounds.WOOD_THUNK, ModAssets.GetSFXVolume() * 0.8f);
						elapsedMessage = 0;
					}
				}
			}
		}
	}
}
