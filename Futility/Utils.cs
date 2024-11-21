using FUtility.Components;
using HarmonyLib;
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

		public static float GetSFXVolume() => KPlayerPrefs.GetFloat("Volume_SFX") * KPlayerPrefs.GetFloat("Volume_Master");
		public static string AssetsPath => Path.Combine(ModPath, "assets");

		public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public static string ConfigPath(string modId) => Path.Combine(Util.RootFolder(), "mods", "config", modId.ToLowerInvariant());

		public const char CENTER = 'O';
		public const char FILLED = 'X';

		private static AccessTools.FieldRef<KBatchedAnimController, KAnimLayering> kAnimLayering;
		private static AccessTools.FieldRef<KAnimLayering, KAnimControllerBase> foregroundController;

		public static string ReplaceLastOccurrence(string source, string find, string replace)
		{
			int place = source.LastIndexOf(find);

			if (place == -1)
				return source;

			return source.Remove(place, find.Length).Insert(place, replace);
		}

		public static void FixFacadeLayers(GameObject go)
		{
			var kbac = go.GetComponent<KBatchedAnimController>();

			if (kAnimLayering == null) kAnimLayering = AccessTools.FieldRefAccess<KBatchedAnimController, KAnimLayering>("layering");
			if (foregroundController == null) AccessTools.FieldRefAccess<KAnimLayering, KAnimControllerBase>("foregroundController");

			if (kAnimLayering == null || foregroundController == null)
				return;

			var layering = kAnimLayering(kbac);

			if (layering == null)
				return;

			(foregroundController(layering) as KBatchedAnimController).SwapAnims(kbac.AnimFiles);

			// Rehide the symbols from the new foreground animation
			layering.HideSymbols();
		}

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

		public static void RegisterBatchTag(KAnimGroupFile kAnimGroupFile, int taghash, HashSet<HashedString> swaps)
		{
			var groups = kAnimGroupFile.GetData();
			var swapAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(taghash));

			// remove the wrong group
			groups.RemoveAll(g => swaps.Contains(g.animNames[0]));

			foreach (var swap in swaps)
			{
				// readd to correct group
				var anim = global::Assets.GetAnim(swap);

				swapAnimsGroup.animFiles.Add(anim);
				swapAnimsGroup.animNames.Add(anim.name);
			}
		}
	}
}
