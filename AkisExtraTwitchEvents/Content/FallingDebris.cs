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

		public override void OnSpawn()
		{
			kbac.Offset = offsetPosition;
			base.OnSpawn();

			RandomizeMassAndTemperature();
			selectable.enabled = offsetPosition.x == 0.0 && offsetPosition.y == 0.0;
		}

		public virtual void RandomizeVelocity()
		{
			float angle = Random.Range(spawnAngleMin, spawnAngleMax);
			float f = angle * Mathf.PI / 180.0f;
			float speed = Random.Range(speedMin, speedMax);

			velocity = new Vector2(-Mathf.Cos(f) * speed, Mathf.Sin(f) * speed);

			Log.Assert("kbac", kbac);
			kbac.Rotation = -angle - 90.0f;
		}

		public void RandomizeMassAndTemperature()
		{
			float mass = Random.Range(massMin, massMax);
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
			int depthOfElement = 0;

			for (int cell1 = Grid.CellBelow(cell); Grid.IsValidCellInWorld(cell1, world) && Grid.Element[cell1] == element; cell1 = Grid.CellBelow(cell1))
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
			int depthOfElement = GetDepthOfElement(cell1, element, world);
			int num2 = Mathf.Min(addTiles, Mathf.Clamp(Mathf.RoundToInt(addTiles), 1, addTiles));

			var valid_cells = HashSetPool<int, Comet>.Allocate();
			var visited_cells = HashSetPool<int, Comet>.Allocate();

			var queue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
			int x1 = -1;
			int x2 = 1;

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

			GameUtil.FloodFillConditional(queue, condition, visited_cells, valid_cells, 10);
			float mass = num2 > 0 ? addTileMass / addTiles : 1f;

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

			valid_cells.Recycle();
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

				Vector2 vector2_1 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * -0.1f;
				Vector2 vector2_2 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * 1.1f;
				Vector3 position = transform.GetPosition();
				Vector3 nextPosition = position + new Vector3(velocity.x * dt, velocity.y * dt, 0.0f);

				if (nextPosition.x < (double)vector2_1.x || vector2_2.x < (double)nextPosition.x || nextPosition.y < (double)vector2_1.y)
					Util.KDestroyGameObject(gameObject);

				int cell = Grid.PosToCell(this);
				int previousCell = Grid.PosToCell(previousPosition);

				if (cell != previousCell)
				{
					if (Grid.IsValidCell(cell) && Grid.Solid[cell])
					{
						PrimaryElement component = GetComponent<PrimaryElement>();

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
