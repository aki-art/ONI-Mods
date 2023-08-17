using FMOD.Studio;
using FUtility.Components;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using static ComplexRecipe;
using Random = UnityEngine.Random;

namespace FUtility
{
	public class Utils
	{
		public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public static string ConfigPath(string modId) => Path.Combine(Util.RootFolder(), "mods", "config", modId.ToLowerInvariant());

		private static MethodInfo m_RegisterDevTool;

		public static CellOffset[] MakeCellOffsets(int width, int height, int offsetX = 0, int offsetY = 0)
		{
			var result = new CellOffset[width * height];

			for (int x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					result[x * y + y] = new CellOffset(x + offsetX, y + offsetY);
				}
			}

			return result;
		}

		public const char CENTER = 'O';
		public const char FILLED = 'X';

		public static List<CellOffset> MakeCellOffsetsFromMap(bool fillCenter, params string[] pattern)
		{
			var xCenter = 0;
			var yCenter = 0;
			var result = new List<CellOffset>();

			for (int y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (int x = 0; x < line.Length; x++)
				{
					if (line[x] == CENTER)
					{
						xCenter = x;
						yCenter = y;

						break;
					}
				}
			}

			for (int y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (int x = 0; x < line.Length; x++)
				{
					if (line[x] == FILLED
						|| (fillCenter && line[x] == CENTER))
						result.Add(new CellOffset(x - xCenter, y - yCenter));
				}
			}

			return result;
		}

		public static void RegisterDevTool<T>(string path) where T : DevTool, new()
		{
			if (m_RegisterDevTool == null)
			{
				m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
				{
					typeof(string)
				});

				if (m_RegisterDevTool == null)
				{
					Log.Warning("DevToolManager.RegisterDevTool couldnt be found, skipping adding dev tools.");
					return;
				}

				if (DevToolManager.Instance == null)
				{
					Log.Warning("DevToolManager.Instance is null, probably trying to call this too early. (try OnAllModsLoaded)");
					return;
				}

				m_RegisterDevTool
					.MakeGenericMethod(typeof(T))
					.Invoke(DevToolManager.Instance, new object[] { path });
			}
		}

		public static string FormatAsLink(string text, string id = null)
		{
			text = STRINGS.UI.StripLinkFormatting(text);

			if (id.IsNullOrWhiteSpace())
			{
				id = text;
				id = id.Replace(" ", "");
			}

			id = id.ToUpperInvariant();
			id = id.Replace("_", "");

			return $"<link=\"{id}\">{text}</link>";
		}

		public static string GetLinkAppropiateFormat(string link)
		{
			return STRINGS.UI.StripLinkFormatting(link)
				.Replace(" ", "")
				.Replace("_", "")
				.ToUpperInvariant();
		}

		/// <summary> Spawns one entity by tag.</summary>
		public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
		{
			var prefab = global::Assets.GetPrefab(tag);

			if (prefab == null) return null;

			var go = GameUtil.KInstantiate(global::Assets.GetPrefab(tag), position, sceneLayer);
			go.SetActive(setActive);

			return go;
		}

		/// <summary> Spawns one entity by tag. </summary>
		public static GameObject Spawn(Tag tag, GameObject atGO, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
		{
			return Spawn(tag, atGO.transform.position, sceneLayer, setActive);
		}

		public static void YeetRandomly(GameObject go, bool onlyUp, float minDistance, float maxDistance, bool rotate, bool stopOnLand = true)
		{
			var vec = Random.insideUnitCircle.normalized;
			if (onlyUp)
			{
				vec.y = Mathf.Abs(vec.y);
			}

			vec += new Vector2(0f, Random.Range(0, 1f));
			vec *= Random.Range(minDistance, maxDistance);

			Yeet(go, minDistance, rotate, vec, stopOnLand);
		}

		public static void YeetAtAngle(GameObject go, float angle, float distance, bool rotate, bool stopOnLand = true)
		{
			var vec = DegreeToVector2(angle) * distance;
			Yeet(go, distance, rotate, vec, stopOnLand);
		}

		private static void Yeet(GameObject go, float distance, bool rotate, Vector2 vec, bool stopOnLand = true)
		{
			if (GameComps.Fallers.Has(go))
				GameComps.Fallers.Remove(go);

			GameComps.Fallers.Add(go, vec);

			if (rotate)
			{
				Rotator rotator = go.AddOrGet<Rotator>();
				rotator.minDistance = distance;
				rotator.SetVec(vec);
				rotator.stopOnLand = stopOnLand;
			}
		}

		public static Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

		public static Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);

		public static ComplexRecipe AddRecipe(string fabricatorID, RecipeElement[] input, RecipeElement[] output, string desc, int sortOrder = 0, float time = 40f)
		{
			string recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

			var recipe = new ComplexRecipe(recipeID, input, output)
			{
				time = time,
				description = desc,
				nameDisplay = RecipeNameDisplay.IngredientToResult,
				fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
			};

			return recipe;
		}

		public static ComplexRecipe AddRecipe(string fabricatorID, RecipeElement input, RecipeElement output, string desc, int sortOrder = 0, float time = 40f)
		{
			var i = new RecipeElement[] { input };
			var o = new RecipeElement[] { output };

			string recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, i, o);

			var recipe = new ComplexRecipe(recipeID, i, o)
			{
				time = time,
				description = desc,
				nameDisplay = RecipeNameDisplay.IngredientToResult,
				fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
			};

			return recipe;
		}

		private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
		{
			if (pInvert)
			{
				return (pY1 - pY2) / (pX1 - pX2);
			}

			return (pX1 - pX2) / (pY1 - pY2);
		}

		private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
		{
			return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
		}

		public static void GetAllVisibleCells(int cell, int range, int depth, List<int> visiblePoints, Func<int, int, bool> DoesOcclude)
		{
			var pos = Grid.CellToXY(cell);

			foreach (DiscreteShadowCaster.Octant octant in Enum.GetValues(typeof(DiscreteShadowCaster.Octant)))
				ScanOctant(pos, range, depth, octant, 1, 0, visiblePoints, DoesOcclude);
		}

		/// copy of <see cref="DiscreteShadowCaster.ScanOctant"/>
		// because i needed to change the condition. but transpiling a methodreplacement have unneccessarily
		// added a performance cost for all Light2D-s, so that's not a reasonable option
		public static void ScanOctant(Vector2I cellPos, int range, int depth, DiscreteShadowCaster.Octant octant, double startSlope, double endSlope, List<int> visiblePoints, Func<int, int, bool> DoesOcclude)
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

					for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope; num2++)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3))
						{
							if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3) && !DoesOcclude(num2 - 1, num3 + 1))
							{
								double slope = GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope, visiblePoints, DoesOcclude);
							}

							continue;
						}

						if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3))
						{
							startSlope = GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
						}

						if (!DoesOcclude(num2, num3 + 1) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3))
							{
								if (num2 + 1 < Grid.WidthInCells && !DoesOcclude(num2 + 1, num3) && !DoesOcclude(num2 + 1, num3 + 1))
								{
									double slope3 = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope3, visiblePoints, DoesOcclude);
								}
							}
							else
							{
								if (num2 + 1 < Grid.WidthInCells && DoesOcclude(num2 + 1, num3))
								{
									startSlope = 0.0 - GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
								}

								if (!DoesOcclude(num2, num3 + 1) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope; num3++)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3))
						{
							if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1) && !DoesOcclude(num2 - 1, num3 - 1))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, DoesOcclude);
							}

							continue;
						}

						if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1))
						{
							startSlope = 0.0 - GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
						}

						if (!DoesOcclude(num2 - 1, num3) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3))
							{
								if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1) && !DoesOcclude(num2 - 1, num3 + 1))
								{
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, DoesOcclude);
								}
							}
							else
							{
								if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1))
								{
									startSlope = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
								}

								if (!DoesOcclude(num2 - 1, num3) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3))
							{
								if (num2 + 1 < Grid.HeightInCells && !DoesOcclude(num2 + 1, num3) && !DoesOcclude(num2 + 1, num3 - 1))
								{
									double slope2 = GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope2, visiblePoints, DoesOcclude);
								}
							}
							else
							{
								if (num2 + 1 < Grid.HeightInCells && DoesOcclude(num2 + 1, num3))
								{
									startSlope = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
								}

								if (!DoesOcclude(num2, num3 - 1) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope; num2++)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3))
						{
							if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3) && !DoesOcclude(num2 - 1, num3 - 1))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false), visiblePoints, DoesOcclude);
							}

							continue;
						}

						if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3))
						{
							startSlope = 0.0 - GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
						}

						if (!DoesOcclude(num2, num3 - 1) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= r2)
						{
							if (DoesOcclude(num2, num3))
							{
								if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1) && !DoesOcclude(num2 + 1, num3 + 1))
								{
									ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, DoesOcclude);
								}
							}
							else
							{
								if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1))
								{
									startSlope = 0.0 - GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
								}

								if (!DoesOcclude(num2 + 1, num3) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

					for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope; num3++)
					{
						if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > r2)
						{
							continue;
						}

						if (DoesOcclude(num2, num3))
						{
							if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1) && !DoesOcclude(num2 + 1, num3 - 1))
							{
								ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, DoesOcclude);
							}

							continue;
						}

						if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1))
						{
							startSlope = GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
						}

						if (!DoesOcclude(num2 + 1, num3) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
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

			if ((depth < range) & !DoesOcclude(num2, num3))
			{
				ScanOctant(cellPos, range, depth + 1, octant, startSlope, endSlope, visiblePoints, DoesOcclude);
			}
		}
	}
}
