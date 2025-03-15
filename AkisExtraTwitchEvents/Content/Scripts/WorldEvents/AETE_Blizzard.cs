using ImGuiNET;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts.WorldEvents
{
	public class AETE_Blizzard : AETE_WorldEvent, ISim33ms
	{

		[Tooltip("The sandstorm is stronger around this radius of the cursor.")]
		[SerializeField] public float intenseRadius;

		[SerializeField] public float baseSnowfallDensityPerSquare100, nearSnowfallDensity;

		private FallingStuffOverlay _snowFallOverlay;

		private int _snowFallRate;
		private Extents _worldExtents;

		public override void Begin()
		{
			Log.Assert("ModAssets.Prefabs.fallingStuffOverlay", ModAssets.Prefabs.fallingStuffOverlay);

			_snowFallOverlay = Instantiate(ModAssets.Prefabs.fallingStuffOverlay).AddComponent<FallingStuffOverlay>();
			_snowFallOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 2.9f };
			_snowFallOverlay.gameObject.SetActive(true);
			_snowFallOverlay.SetScale(0.5f);
			_snowFallOverlay.SetAngleAndSpeed(90f, 0.007f);
			_snowFallOverlay.AlignToWorld();
			_snowFallOverlay.SetColor(Util.ColorFromHex("c4d6f593"));
			_snowFallOverlay.FadeIn(5f);

			base.Begin();
			AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.BLIZZARD, true, false);
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

			UpdateSnowdFallRate();

			if (Stage == WorldEventStage.Active)
			{
				AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.BLIZZARD, true, true);
			}
		}

		private void UpdateSnowdFallRate()
		{
			_snowFallRate = Mathf.CeilToInt((baseSnowfallDensityPerSquare100 / 10000f) * (_worldExtents.width * _worldExtents.height));
			if (_snowFallRate > 50)
			{
				Log.Warning($"Sand fall rate ludicrously high ({_snowFallRate}), restricting.");
				_snowFallRate = 50;
			}
		}

		public override void End()
		{
			base.End();
			if (_snowFallOverlay != null)
				_snowFallOverlay.FadeOut(10f);
			AkisTwitchEvents.Instance.ToggleOverlay(0, OverlayRenderer.BLIZZARD, false, false);
			Util.KDestroyGameObject(gameObject);
		}

		public void Sim33ms(float dt)
		{
			elapsedTime += dt;

			if (Stage == WorldEventStage.Active)
			{
				var originCell = PosUtil.ClampedMouseCell();

				for (int i = 0; i < nearSnowfallDensity; i++)
				{
					var pos = Random.insideUnitCircle * intenseRadius;
					var cell = Grid.OffsetCell(originCell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

					SpawnFallingDebris(cell);
				}

				for (int i = 0; i < _snowFallRate; i++)
				{
					var cell = Grid.XYToCell(
						Random.Range(_worldExtents.x + 1, _worldExtents.x + _worldExtents.width),
						Random.Range(_worldExtents.y + 1, _worldExtents.y + _worldExtents.height));

					SpawnFallingDebris(cell);
				}

				if (elapsedTime > durationInSeconds)
					End();
			}
		}

		private void SpawnFallingDebris(int cell)
		{
			if (!Grid.IsValidCellInWorld(cell, this.GetMyWorldId()) || !GridUtil.IsCellEmpty(cell))
				return;

			var go = FUtility.Utils.Spawn(SnowDropCometConfig.ID, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), setActive: false);

			go.TryGetComponent(out FallingDebris falling);
			falling.spawnAngleMin = -90f;
			falling.spawnAngleMax = -100f;
			go.SetActive(true);
			falling.RandomizeVelocity();
		}

		public override void OnImguiDraw()
		{
			ImGui.DragFloat("Intense Radius", ref intenseRadius, 0.5f, 1, 20);

			if (ImGui.DragFloat("Snowfall Base Rate##snowfallBaseRate", ref baseSnowfallDensityPerSquare100, 1, 0, 200))
			{
				UpdateSnowdFallRate();
			}

			ImGui.DragFloat("Snowfall Near Rate##snowFallNearRate", ref nearSnowfallDensity, 1, 0, 200);

			ImGui.Separator();
			ImGui.Text("Snow Fall");
			ImGui.Separator();
			_snowFallOverlay.OnImguiDraw();
		}
	}
}