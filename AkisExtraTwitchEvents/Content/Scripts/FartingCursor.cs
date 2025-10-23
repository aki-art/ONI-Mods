using ONITwitchLib.Utils;
using System.Collections;
using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class FartingCursor : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public float massPerSecondEmitted;

		public static List<CellOffset> targetTiles = FUtility.Utils.MakeCellOffsetsFromMap(false,
			" X ",
			"XOX",
			" X ");

		private bool _isRunning;
		public override void OnPrefabInit()
		{
			Subscribe(ModEvents.TimeOut, OnTimerTimeOut);
		}

		public void Begin()
		{
			_isRunning = true;
			StartCoroutine(PlayFx());
		}

		private IEnumerator PlayFx()
		{
			while (_isRunning)
			{
				if (!SpeedControlScreen.Instance.IsPaused)
				{
					var cell = PosUtil.ClampedMouseCell();

					var fx = FXHelpers.CreateEffect("aete_methane_explosion_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
					fx.Rotation = Random.Range(0, 360);
					fx.animScale *= Random.Range(0.4f, 0.6f);
					fx.Play("idle");
					fx.destroyOnAnimComplete = true;

					AudioUtil.PlaySound(ModAssets.Sounds.FART_REVERB, ModAssets.GetSFXVolume(), Random.Range(0.6f, 1.4f));

				}

				yield return new WaitForSecondsRealtime(0.2f);
			}
		}

		private void OnTimerTimeOut(object obj)
		{
			if (obj is HashedString id && id == "fart_over")
			{
				StopCoroutine(PlayFx());
				Util.KDestroyGameObject(gameObject);
			}
		}

		public void Sim200ms(float dt)
		{
			if (!_isRunning)
				return;

			var cell = PosUtil.ClampedMouseCell();

			SimMessages.ReplaceElement(cell, SimHashes.Methane, Toucher.toucherEvent, massPerSecondEmitted * dt);
			ExplosionUtil.Explode(cell, 1f, new Vector2(8, 14), targetTiles);
		}
	}
}
