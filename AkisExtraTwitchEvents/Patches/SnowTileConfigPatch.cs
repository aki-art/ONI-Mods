using HarmonyLib;
using Twitchery.Content;
using UnityEngine;

namespace Twitchery.Patches
{
	public class SnowTileConfigPatch
	{
		[HarmonyPatch(typeof(SnowTileConfig), "CreateBuildingDef")]
		public class SnowTileConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				var currentCat = __result.MaterialCategory[0];
				if (currentCat == SimHashes.Snow.ToString())
				{
					__result.MaterialCategory = [TTags.buildingSnow.ToString()];
				}
				else
				{
					var element = ElementLoader.FindElementByName(currentCat);
					var isElement = element != null;
					var isItem = Assets.TryGetPrefab(currentCat) != null;

					if (!isElement && !isItem)
					{
						var newElement = ElementLoader.FindElementByHash(Elements.YellowSnow);
						newElement.oreTags = newElement.oreTags.AddToArray(currentCat);
					}
					else
					{
						Log.Warning($"Someone has changed Snow Tile's construction material to {currentCat}, cannot add Yellow Snow as an option :(.");
					}
				}
			}
		}

		private static bool ReplaceElement(GameObject instance)
		{
			if (instance.TryGetComponent(out PrimaryElement primaryElement))
			{
				if (primaryElement.ElementID == Elements.YellowSnow)
				{
					primaryElement.SetElement(Elements.StableYellowSnow);

					if (instance.TryGetComponent(out Deconstructable deconstructable))
						deconstructable.constructionElements = [Elements.StableYellowSnow.Tag];

					return false;
				}
			}


			return true;
		}

		[HarmonyPatch(typeof(SnowTileConfig), "BuildingComplete_OnSpawn")]
		public class SnowTileConfig_BuildingComplete_OnSpawn_Patch
		{
			public static bool Prefix(GameObject instance) => ReplaceElement(instance);
		}


		[HarmonyPatch(typeof(SnowTileConfig), "BuildingComplete_OnInit")]
		public class SnowTileConfig_BuildingComplete_OnInit_Patch
		{
			public static bool Prefix(GameObject instance) => ReplaceElement(instance);
		}
	}
}
