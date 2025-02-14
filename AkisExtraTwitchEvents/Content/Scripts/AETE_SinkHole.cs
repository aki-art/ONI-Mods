using FUtility;
using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	internal class AETE_SinkHole : AETE_WorldEvent, ISim33ms, IImguiDebug
	{
		private const float RADIUS_MULTIPLIER = 20f;
		private const float MAGNITUDE_BIAS = 0.45f;

		[SerializeField] public float duration = -1;

		[MyCmpReq] public ItemSucker sucker;

		private static float spaceBias = 0.2f;

		private float MinPower => Mathf.Min(1, (int)AkisTwitchEvents.MaxDanger * 0.5f);

		private float MaxPower => (int)AkisTwitchEvents.MaxDanger * 2f;

		public override void OnSpawn() // gets called when this object spawns  
		{
			Randomize();
			affectedCells = GetCircle(radius, 0);
			base.OnSpawn();
		}

		public override void Begin()
		{
			base.Begin();
			sucker.SetRange(radius);
			sucker.suck = true;
			AudioUtil.PlaySound(ModAssets.Sounds.VACUUM, transform.position, ModAssets.GetSFXVolume());
		}

		public override void End()
		{
			base.End();
			sucker.suck = false;

			Util.KDestroyGameObject(gameObject);
		}

		public void Randomize()
		{
			if (randomize)
			{
				float powerRange = MaxPower - MinPower;
				power = FUtility.Utils.Bias(Random.value, MAGNITUDE_BIAS) / powerRange + MinPower;
				power = Mathf.Clamp(power, MinPower, MaxPower);
				radius = Random.Range(3, Mathf.Min(Mathf.FloorToInt(Power * RADIUS_MULTIPLIER), 8));
			}
		}

		public Dictionary<int, float> GetCircle(int closeRange = 10, int falloffRange = 5)
		{
			Dictionary<int, float> cells = [];
			AddCellRange(cells, transform.position, power, closeRange, falloffRange);

			return cells;
		}

		private static void AddCellRange(Dictionary<int, float> cells, Vector3 position, float power, int innerRadius, int outerRadius)
		{
			int radius = innerRadius + outerRadius;
			var range = ProcGen.Util.GetFilledCircle(position, radius);

			foreach (Vector2I pos in range)
			{
				int cell = Grid.XYToCell(Grid.ClampX(pos.x), pos.y);
				if (Grid.IsValidBuildingCell(cell))
					cells[cell] = GetPower(position, power, innerRadius, outerRadius, pos);
			}
		}

		private static float GetPower(Vector3 position, float power, int innerRadius, int outerRadius, Vector2I pos)
		{
			float distance = Vector2.Distance(position, pos);
			float powerMultiplier = distance > innerRadius ? 1 - (distance - innerRadius) / outerRadius : 1f;
			return power * powerMultiplier;
		}

		public void Sim33ms(float dt)
		{
			elapsedTime += dt;

			if (elapsedTime >= duration)
			{
				End();
				return;
			}

			for (int i = 0; i < Random.Range(0, 2); i++)
			{
				var targetCell = affectedCells.GetRandom();
				var cell = targetCell.Key;

				if (!Grid.IsValidCell(cell))
					continue;

				var damage = DamageTile(targetCell.Key, targetCell.Value, true, 0.1f);

				if (damage == 0 && !Grid.IsSolidCell(cell))
				{
					var distance = Grid.GetCellDistance(Grid.PosToCell(this), cell);

					if (Random.value * distance > spaceBias)
					{
						AkisTwitchEvents.Instance.AddZoneTypeOverride(targetCell.Key, ProcGen.SubWorld.ZoneType.Space);
						var sound = GlobalAssets.GetSound("MeteorDamage_Rock");

						if (CameraController.Instance && CameraController.Instance.IsAudibleSound(transform.position, sound))
						{
							KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(transform.position), 0.7f);
						}
					}
				}
			}
		}

		public void OnImgui()
		{
			ImGui.DragFloat("Spave Bias", ref spaceBias);
		}
	}
}
