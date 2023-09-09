using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class PlanScreenPatch
	{
		// Makes the Copy Building button target default glass in the build menu.
		[HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
		public static class PlanScreen_OnClickCopyBuilding_Patch
		{
			public static bool Prefix()
			{
				if (SelectTool.Instance.selected == null)
					return true;

				if (SelectTool.Instance.selected.TryGetComponent(out Building building))
				{
					if (building.Def.BuildingComplete.HasTag(ModAssets.Tags.stainedGlass) && building.Def.name != DefaultStainedGlassTileConfig.DEFAULT_ID)
					{
						OpenBuildMenu(building);
						return false;
					}
				}

				return true;
			}

			private static void OpenBuildMenu(Building building)
			{
				foreach (var planInfo in TUNING.BUILDINGS.PLANORDER)
				{
					foreach (var buildingAndSubCategory in planInfo.buildingAndSubcategoryData)
					{
						if (buildingAndSubCategory.Key == DefaultStainedGlassTileConfig.DEFAULT_ID)
						{
							var defaultStainedDef = Assets.GetBuildingDef(DefaultStainedGlassTileConfig.DEFAULT_ID);

							PlanScreen.Instance.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
							var gameObject = PlanScreen.Instance.activeCategoryBuildingToggles[defaultStainedDef].gameObject;

							PlanScreen.Instance.OnSelectBuilding(gameObject, defaultStainedDef);

							if (PlanScreen.Instance.ProductInfoScreen == null)
								return;

							PlanScreen.Instance.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);

							return;
						}
					}
				}
			}
		}
	}
}
