using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content
{
	// basically just a simplified Comet
	public class FallingDebris : KMonoBehaviour, ISim33ms
	{
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private KSelectable selectable;

		public SpawnFXHashes explosionEffectHash;
		public int splashRadius = 1;
		public int addTiles;
		public float massMin, massMax;
		public float tempMinC, tempMaxC;
		public float spawnAngleMin, spawnAngleMax;
		public float speedMin, speedMax;
		public string impactSound;
		public bool destroyOnExplode = true;

		[Serialize] protected Vector2 velocity;
		[Serialize] protected Vector3 offsetPosition;
		[Serialize] private float temperature;

		private Vector3 previousPosition;
		private bool hasExploded;
		private float age;
		protected float addTileMass;

		private List<int> validCells = [];

		public override void OnSpawn()
		{
			kbac.Offset = offsetPosition;
			base.OnSpawn();

			RandomizeMassAndTemperature();
			selectable.enabled = offsetPosition.x == 0.0 && offsetPosition.y == 0.0;
		}

		public virtual void RandomizeVelocity()
		{
			var angle = Random.Range(spawnAngleMin, spawnAngleMax);
			var f = angle * Mathf.PI / 180.0f;
			var speed = Random.Range(speedMin, speedMax);

			velocity = new Vector2(-Mathf.Cos(f) * speed, Mathf.Sin(f) * speed);

			Log.Assert("kbac", kbac);
			kbac.Rotation = -angle - 90.0f;
		}

		public void RandomizeMassAndTemperature()
		{
			var mass = Random.Range(massMin, massMax);
			if (TryGetComponent(out PrimaryElement primaryElement))
			{
				primaryElement.Mass = mass;
				temperature = GameUtil.GetTemperatureConvertedToKelvin(Random.Range(tempMinC, tempMaxC), GameUtil.TemperatureUnit.Celsius);
				primaryElement.Temperature = temperature;
			}

			if (addTiles > 0)
				addTileMass = mass * Random.Range(0.95f, 0.98f);
		}

		protected int GetDepthOfElement(int cell, Element element, int world)
		{
			var depthOfElement = 0;

			for (var cell1 = Grid.CellBelow(cell); Grid.IsValidCellInWorld(cell1, world) && Grid.Element[cell1] == element; cell1 = Grid.CellBelow(cell1))
				++depthOfElement;

			return depthOfElement;
		}

		protected virtual void DepositTiles(
			int cell1,
			Element element,
			int world,
			int prev_cell,
			float temperature)
		{
			var depthOfElement = GetDepthOfElement(cell1, element, world);
			var num2 = Mathf.Min(addTiles, Mathf.Clamp(Mathf.RoundToInt(addTiles), 1, addTiles));

			var visited_cells = HashSetPool<int, Comet>.Allocate();

			var queue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
			var x1 = -1;
			var x2 = 1;

			if (velocity.x < 0.0)
			{
				x1 *= -1;
				x2 *= -1;
			}

			queue.Enqueue(new GameUtil.FloodFillInfo()
			{
				cell = prev_cell,
				depth = 0
			});

			queue.Enqueue(new GameUtil.FloodFillInfo()
			{
				cell = Grid.OffsetCell(prev_cell, new CellOffset(x1, 0)),
				depth = 0
			});

			queue.Enqueue(new GameUtil.FloodFillInfo()
			{
				cell = Grid.OffsetCell(prev_cell, new CellOffset(x2, 0)),
				depth = 0
			});

			bool condition(int cell2) => Grid.IsValidCellInWorld(cell2, world) && !Grid.Solid[cell2];

			GameUtil.FloodFillConditional(queue, condition, visited_cells, validCells, 10);
			var mass = num2 > 0 ? addTileMass / addTiles : 1f;

			foreach (var gameCell in validCells)
			{
				if (num2 > 0)
				{
					SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, byte.MaxValue, 0);
					--num2;
				}
				else
					break;
			}

			/*			if (element.HasTag(GameTags.Unstable))
						{
							UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
							foreach (int cell in (HashSet<int>)valid_cells)
							{
								if (num2 > 0)
								{
									component.Spawn(cell, element, mass, temperature, byte.MaxValue, 0);
									--num2;
								}
								else
									break;
							}
						}
						else
						{
							foreach (int gameCell in (HashSet<int>)valid_cells)
							{
								if (num2 > 0)
								{
									SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, byte.MaxValue, 0);
									--num2;
								}
								else
									break;
							}
						}*/

			validCells.Clear();
			visited_cells.Recycle();
			queue.Recycle();
		}

		public void Sim33ms(float dt)
		{
			if (hasExploded)
				return;

			if (offsetPosition.y > 0.0)
			{
				offsetPosition += new Vector3(velocity.x * dt, velocity.y * dt, 0.0f);
				kbac.Offset = offsetPosition;
			}
			else
			{
				if (kbac.Offset != Vector3.zero)
					kbac.Offset = Vector3.zero;

				if (!selectable.enabled)
					selectable.enabled = true;

				var vector2_1 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * -0.1f;
				var vector2_2 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * 1.1f;
				var position = transform.GetPosition();
				var nextPosition = position + new Vector3(velocity.x * dt, velocity.y * dt, 0.0f);

				if (nextPosition.x < (double)vector2_1.x || vector2_2.x < (double)nextPosition.x || nextPosition.y < (double)vector2_1.y)
					Util.KDestroyGameObject(gameObject);

				var cell = Grid.PosToCell(this);
				var previousCell = Grid.PosToCell(previousPosition);

				if (cell != previousCell)
				{
					if (Grid.IsValidCell(cell) && Grid.Solid[cell])
					{
						var component = GetComponent<PrimaryElement>();

						int world = Grid.WorldIdx[cell];
						DepositTiles(cell, component.Element, world, previousCell, temperature);
						hasExploded = true;

						if (destroyOnExplode)
							Util.KDestroyGameObject(gameObject);

						return;
					}
				}

				previousPosition = position;
				transform.SetPosition(nextPosition);
			}

			age += dt;
		}
	}
}
