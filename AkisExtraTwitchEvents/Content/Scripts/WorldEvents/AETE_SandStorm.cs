using ImGuiNET;
using KSerialization;
using ONITwitchLib.Utils;
using System;
using Twitchery.Content.Defs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts.WorldEvents
{
	public class AETE_SandStorm : AETE_WorldEvent, ISim33ms
	{
		[Tooltip("The sandstorm is stronger around this radius of the cursor.")]
		[SerializeField] public float intenseRadius;

		[SerializeField] public int minSmallWorms, maxSmallWorms;
		[SerializeField] public bool spawnBigWorm;
		[SerializeField] public float baseSandfallDensityPerSquare100, nearSandfallDensity;

		private FallingStuffOverlay _sandFallOverlay;

		private int _sandFallRate;
		private Extents _worldExtents;
		[Serialize] private QueuedWormData _queuedWormData;

		[Serializable]
		private struct QueuedWormData
		{
			public Tag worm;
			public Vector3 spawnPosition;
			public float awakeTime;
		}

		public override void Begin()
		{
			Log.Assert("ModAssets.Prefabs.fallingStuffOverlay", ModAssets.Prefabs.fallingStuffOverlay);

			_sandFallOverlay = Instantiate(ModAssets.Prefabs.fallingStuffOverlay).AddComponent<FallingStuffOverlay>();
			_sandFallOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 2.9f };
			_sandFallOverlay.gameObject.SetActive(true);
			_sandFallOverlay.SetScale(0.5f);
			_sandFallOverlay.SetAngleAndSpeed(66f, 0.018f);
			_sandFallOverlay.AlignToWorld();
			_sandFallOverlay.SetColor(Util.ColorFromHex("9F884E93"));
			_sandFallOverlay.FadeIn(5f);

			base.Begin();
			AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.SAND_STORM, true, false);

			if (spawnBigWorm)
			{
				// way off screen
				var pos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()) + ((Vector3)Random.insideUnitCircle.normalized * 200f) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };

				FUtility.Utils.Spawn(BigWormConfig.ID, pos);
			}
		}

		protected override void Initialize()
		{
			base.Initialize();

			var world = this.GetMyWorld();

			_worldExtents = new Extents(
				(int)world.minimumBounds.x,
				(int)world.minimumBounds.y,
				world.Width,
				world.Height);

			UpdateSandFallRate();

			if (Stage == WorldEventStage.Active)
			{
				AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.SAND_STORM, true, true);
			}
		}

		private void UpdateSandFallRate()
		{
			_sandFallRate = Mathf.CeilToInt((baseSandfallDensityPerSquare100 / 10000f) * (_worldExtents.width * _worldExtents.height));
			if (_sandFallRate > 50)
			{
				Log.Warning($"Sand fall rate ludicrously high ({_sandFallRate}), restricting.");
				_sandFallRate = 50;
			}
		}

		public override void End()
		{
			base.End();
			if (_sandFallOverlay != null)
				_sandFallOverlay.FadeOut(10f);
			AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.SAND_STORM, false, false);
			Util.KDestroyGameObject(gameObject);
		}

		public void Sim33ms(float dt)
		{
			elapsedTime += dt;

			if (Stage == WorldEventStage.Active)
			{
				var originCell = PosUtil.ClampedMouseCell();

				for (int i = 0; i < nearSandfallDensity; i++)
				{
					var pos = Random.insideUnitCircle * intenseRadius;
					var cell = Grid.OffsetCell(originCell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

					SpawnSandFallingDebris(cell);
				}

				for (int i = 0; i < _sandFallRate; i++)
				{
					var cell = Grid.XYToCell(
						Random.Range(_worldExtents.x + 1, _worldExtents.x + _worldExtents.width),
						Random.Range(_worldExtents.y + 1, _worldExtents.y + _worldExtents.height));

					SpawnSandFallingDebris(cell);
				}

				if (elapsedTime > durationInSeconds)
					End();
			}
		}

		private void SpawnSandFallingDebris(int cell)
		{
			if (!Grid.IsValidCellInWorld(cell, this.GetMyWorldId()) || !GridUtil.IsCellEmpty(cell))
				return;

			var go = FUtility.Utils.Spawn(SandDropCometConfig.ID, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), setActive: false);

			go.TryGetComponent(out FallingDebris falling);
			falling.spawnAngleMin = -105f;
			falling.spawnAngleMax = -115f;
			go.SetActive(true);
			falling.RandomizeVelocity();
		}

		public override void OnImguiDraw()
		{
			ImGui.DragFloat("Intense Radius", ref intenseRadius, 0.5f, 1, 20);

			if (ImGui.DragFloat("Sandfall Base Rate", ref baseSandfallDensityPerSquare100, 1, 0, 200))
			{
				UpdateSandFallRate();
			}

			ImGui.DragFloat("Sandfall Near Rate", ref nearSandfallDensity, 1, 0, 200);

			ImGui.Separator();
			ImGui.Text("Sand Fall");
			ImGui.Separator();
			_sandFallOverlay.OnImguiDraw();
		}
	}
}