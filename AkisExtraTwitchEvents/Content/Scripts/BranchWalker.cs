using KSerialization;
using System;
using System.Collections.Generic;
using Twitchery.Content.Defs;
using Twitchery.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class BranchWalker : KMonoBehaviour, ISim33ms
	{
		[Serialize] public bool isRunning;
		[Serialize] public int currentCell;
		[Serialize] public int targetCell;
		[Serialize] public int length = 0;
		[Serialize] public List<Vector2I> growthPath;
		[Serialize] public DiscreteShadowCaster.Octant direction;
		[Serialize] public bool initialized;
		[Serialize] public int currentStep;
		[Serialize] public int stepsToTake;
		[Serialize] public int generation;
		[Serialize] public List<int> leaves;

		[SerializeField][Serialize] public float branchOffChance;
		[SerializeField][Serialize] public int maxDistance;
		[SerializeField][Serialize] public SimHashes barkElement;
		[SerializeField][Serialize] public float barkMass;
		[SerializeField][Serialize] public int maxComplexity;

		[SerializeField][Serialize] public int foliageRadius;
		[SerializeField][Serialize] public SimHashes foliageElement;
		[SerializeField][Serialize] public float foliageMass;
		[SerializeField][Serialize][Range(0, 1)] public float foliageDensity;

		[SerializeField] public int stepRange;
		[SerializeField] public int minimumIdealBlocks;
		[SerializeField][Range(0, 1)] public float nextStepChance;
		[SerializeField] public int maximumSteps, minimumSteps;
		[SerializeField] public int maxHardness;

		public List<int> visibleCells;
		public bool overrideDirection;

		private static readonly Tag[] avoidTags = new[]
		{
			GameTags.CreatureBrain,
			GameTags.DupeBrain,
			GameTags.Robot
		};

		public override void OnSpawn()
		{
			base.OnSpawn();

			visibleCells = new();
			leaves = new();

			if (!initialized)
			{
				growthPath = new();
				stepsToTake = Random.Range(minimumSteps, maximumSteps);
				currentCell = Grid.PosToCell(this);
				if (!overrideDirection)
					direction = FindBestDirection();
				FindNewTarget();
				initialized = true;
			}
		}

		public void Generate()
		{
			isRunning = true;
		}

		public void SetDirection(DiscreteShadowCaster.Octant direction)
		{
			this.direction = direction;
			overrideDirection = true;
		}

		private DiscreteShadowCaster.Octant FindBestDirection()
		{
			var result = DiscreteShadowCaster.Octant.N_NW;
			var max = -99;

			foreach (DiscreteShadowCaster.Octant octant in Enum.GetValues(typeof(DiscreteShadowCaster.Octant)))
			{
				GetVisibleCells(currentCell, visibleCells, stepRange, octant);
				var count = visibleCells.Count;
				if (count > max)
				{
					max = count;
					max = count;
					result = octant;
				}
			}

			return result;
		}

		public void Sim33ms(float dt)
		{
			if (!isRunning)
				return;

			if (maxComplexity >= generation && Random.value < branchOffChance)
			{
				var branch = FUtility.Utils.Spawn(BranchWalkerConfig.ID, gameObject);

				var directionChange = Random.value > 0.5f ? 1 : -1;
				var newDirection = (int)direction + directionChange;
				if (newDirection < 0) newDirection = 7;
				if (newDirection > 7) newDirection = 0;

				var branchWalker = branch.GetComponent<BranchWalker>();
				branchWalker.SetDirection((DiscreteShadowCaster.Octant)newDirection);
				branchWalker.generation = generation + 1;
				branchWalker.maxComplexity = maxComplexity - 1;

				branchWalker.Generate();
			}

			if (currentStep > stepsToTake)
			{
				Finish();
				return;
			}

			if (growthPath.Count == 0 && maxDistance > 1)
			{
				FindNewTarget();
				currentStep++;
			}

			if (targetCell == -1 || growthPath.Count == 0)
			{
				Finish();
				return;
			}

			var nextCell = Grid.PosToCell(growthPath[0]);

			SpawnBlock(nextCell, barkElement, barkMass);

			length++;

			growthPath.RemoveAt(0);
			currentCell = nextCell;
		}

		public void Finish()
		{
			SpawnFoliage();
			Util.KDestroyGameObject(this);
		}

		private void SpawnBlock(int cell, SimHashes element, float mass)
		{
			if (!Grid.IsValidCell(cell))
				return;

			// skip occupied cells. while the branch tries to not grow into these, if one was placed since the start it might
			// run into one anyway
			if (DoesOcclude(cell, maxHardness))
				return;

			// todo: skip or displace dupes

			if (HasDupeOrCritter(cell))
				return;

			if (AGridUtil.PlaceElement(cell, element, mass))
			{
				AudioUtil.PlaySound(
					 element == Elements.FakeLumber
					 ? ModAssets.Sounds.WOOD_THUNK
					 : ModAssets.Sounds.LEAF, Grid.CellToPos(cell), ModAssets.GetSFXVolume());
			}

		}

		private static bool HasDupeOrCritter(int cell)
		{
			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				Extents.OneCell(cell),
				GameScenePartitioner.Instance.pickupablesLayer,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is Pickupable pickupable)
				{
					if (pickupable.HasAnyTags(avoidTags))
						return true;
				}
			}

			return false;
		}

		private void FindNewTarget()
		{
			GetVisibleCells(currentCell, visibleCells, stepRange, direction);
			targetCell = visibleCells.Count > 0 ? visibleCells.GetRandom() : -1;
			PopulateGrowthpath();
		}

		private void PopulateGrowthpath()
		{
			growthPath = ProcGen.Util.GetLine(Grid.CellToPos(currentCell), Grid.CellToPos(targetCell));
		}

		private void SpawnFoliage()
		{
			visibleCells.Clear();
			DiscreteShadowCaster.GetVisibleCells(currentCell, visibleCells, foliageRadius, LightShape.Circle, false);

			foreach (var cell in visibleCells)
				if (Random.value < foliageDensity)
					SpawnBlock(cell, foliageElement, foliageMass);
		}

		public void GetVisibleCells(int cell, List<int> visiblePoints, int range, DiscreteShadowCaster.Octant direction)
		{
			visiblePoints.Clear();
			var xy = Grid.CellToXY(cell);
			ScanOctant(xy, range, 2, direction, 1, 0, visiblePoints, maxHardness);
		}

		public static bool DoesOcclude(int x, int y, int maxHardness)
		{
			return DoesOcclude(Grid.XYToCell(x, y), maxHardness);
		}

		public static bool DoesOcclude(int cell, int maxHardness)
		{
			if (!ONITwitchLib.Utils.GridUtil.IsCellFoundationEmpty(cell)
				&& Grid.Element[cell].hardness > maxHardness)
				return true;

			if (HasDupeOrCritter(cell))
				return true;

			return false;
		}

		/// copy of <see cref="DiscreteShadowCaster.ScanOctant"/>
		// because i needed to change the condition. but transpiling a methodreplacement have unneccessarily
		// added a performance cost for all Light2D-s, so that's not a reasonable option
		public static void ScanOctant(Vector2I cellPos, int range, int depth, DiscreteShadowCaster.Octant octant, double startSlope, double endSlope, List<int> visiblePoints, int maxHardness = 2)
		{
			int r2 = range * range;
			int num2 = 0;
			int num3 = 0;

			switch (octant)
			{
				case DiscreteShadowCaster.Octant.S_SW:
					num3 = cellPos.y - depth;
					if (num3 < 0)
					{
						return;
					}

					num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num2 < 0)
					{
						num2 = 0;
					}

					for (; DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope; num2++)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3, maxHardness))
						{
							if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3, maxHardness) && !DoesOcclude(num2 - 1, num3 + 1, maxHardness))
							{
								double slope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope, visiblePoints, maxHardness);
							}

							continue;
						}

						if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, maxHardness))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
						}

						if (!DoesOcclude(num2, num3 + 1, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}

					num2--;
					break;
				case DiscreteShadowCaster.Octant.S_SE:
					num3 = cellPos.y - depth;
					if (num3 < 0)
					{
						return;
					}

					num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num2 >= Grid.WidthInCells)
					{
						num2 = Grid.WidthInCells - 1;
					}

					while (DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3, maxHardness))
							{
								if (num2 + 1 < Grid.WidthInCells && !DoesOcclude(num2 + 1, num3, maxHardness) && !DoesOcclude(num2 + 1, num3 + 1, maxHardness))
								{
									double slope3 = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope3, visiblePoints, maxHardness);
								}
							}
							else
							{
								if (num2 + 1 < Grid.WidthInCells && DoesOcclude(num2 + 1, num3, maxHardness))
								{
									startSlope = 0.0 - DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
								}

								if (!DoesOcclude(num2, num3 + 1, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
								{
									visiblePoints.Add(Grid.XYToCell(num2, num3));
								}
							}
						}

						num2--;
					}

					num2++;
					break;
				case DiscreteShadowCaster.Octant.E_SE:
					num2 = cellPos.x + depth;
					if (num2 >= Grid.WidthInCells)
					{
						return;
					}

					num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num3 < 0)
					{
						num3 = 0;
					}

					for (; DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope; num3++)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3, maxHardness))
						{
							if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1, maxHardness) && !DoesOcclude(num2 - 1, num3 - 1, maxHardness))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, maxHardness);
							}

							continue;
						}

						if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, maxHardness))
						{
							startSlope = 0.0 - DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
						}

						if (!DoesOcclude(num2 - 1, num3, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}

					num3--;
					break;
				case DiscreteShadowCaster.Octant.E_NE:
					num2 = cellPos.x + depth;
					if (num2 >= Grid.WidthInCells)
					{
						return;
					}

					num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num3 >= Grid.HeightInCells)
					{
						num3 = Grid.HeightInCells - 1;
					}

					while (DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3, maxHardness))
							{
								if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1, maxHardness) && !DoesOcclude(num2 - 1, num3 + 1, maxHardness))
								{
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, maxHardness);
								}
							}
							else
							{
								if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, maxHardness))
								{
									startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
								}

								if (!DoesOcclude(num2 - 1, num3, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
								{
									visiblePoints.Add(Grid.XYToCell(num2, num3));
								}
							}
						}

						num3--;
					}

					num3++;
					break;
				case DiscreteShadowCaster.Octant.N_NE:
					num3 = cellPos.y + depth;
					if (num3 >= Grid.HeightInCells)
					{
						return;
					}

					num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num2 >= Grid.WidthInCells)
					{
						num2 = Grid.WidthInCells - 1;
					}

					while (DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3, maxHardness))
							{
								if (num2 + 1 < Grid.HeightInCells && !DoesOcclude(num2 + 1, num3, maxHardness) && !DoesOcclude(num2 + 1, num3 - 1, maxHardness))
								{
									double slope2 = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope2, visiblePoints, maxHardness);
								}
							}
							else
							{
								if (num2 + 1 < Grid.HeightInCells && DoesOcclude(num2 + 1, num3, maxHardness))
								{
									startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
								}

								if (!DoesOcclude(num2, num3 - 1, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
								{
									visiblePoints.Add(Grid.XYToCell(num2, num3));
								}
							}
						}

						num2--;
					}

					num2++;
					break;
				case DiscreteShadowCaster.Octant.N_NW:
					num3 = cellPos.y + depth;
					if (num3 >= Grid.HeightInCells)
					{
						return;
					}

					num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num2 < 0)
					{
						num2 = 0;
					}

					for (; DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope; num2++)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3, maxHardness))
						{
							if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3, maxHardness) && !DoesOcclude(num2 - 1, num3 - 1, maxHardness))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false), visiblePoints, maxHardness);
							}

							continue;
						}

						if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, maxHardness))
						{
							startSlope = 0.0 - DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
						}

						if (!DoesOcclude(num2, num3 - 1, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}

					num2--;
					break;
				case DiscreteShadowCaster.Octant.W_NW:
					num2 = cellPos.x - depth;
					if (num2 < 0)
					{
						return;
					}

					num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num3 >= Grid.HeightInCells)
					{
						num3 = Grid.HeightInCells - 1;
					}

					while (DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3, maxHardness))
							{
								if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1, maxHardness) && !DoesOcclude(num2 + 1, num3 + 1, maxHardness))
								{
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, maxHardness);
								}
							}
							else
							{
								if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, maxHardness))
								{
									startSlope = 0.0 - DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
								}

								if (!DoesOcclude(num2 + 1, num3, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
								{
									visiblePoints.Add(Grid.XYToCell(num2, num3));
								}
							}
						}

						num3--;
					}

					num3++;
					break;
				case DiscreteShadowCaster.Octant.W_SW:
					num2 = cellPos.x - depth;
					if (num2 < 0)
					{
						return;
					}

					num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
					if (num3 < 0)
					{
						num3 = 0;
					}

					for (; DiscreteShadowCaster.GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope; num3++)
					{
						if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3, maxHardness))
						{
							if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1, maxHardness) && !DoesOcclude(num2 + 1, num3 - 1, maxHardness))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, maxHardness);
							}

							continue;
						}

						if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, maxHardness))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
						}

						if (!DoesOcclude(num2 + 1, num3, maxHardness) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}

					num3--;
					break;
			}

			if (num2 < 0)
			{
				num2 = 0;
			}
			else if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}

			if (num3 < 0)
			{
				num3 = 0;
			}
			else if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}

			if ((depth < range) & !DoesOcclude(num2, num3, maxHardness))
			{
				ScanOctant(cellPos, range, depth + 1, octant, startSlope, endSlope, visiblePoints, maxHardness);
			}
		}
	}
}
