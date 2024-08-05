using DecorPackB.Content.Defs.Buildings;
using HarmonyLib;

namespace DecorPackB.Patches
{
	internal class PlanScreenPatch
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
					if (building.Def.BuildingComplete.HasTag(ModTags.floorLamp) && building.Def.name != DefaultFloorLampConfig.DEFAULT_ID)
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
						if (buildingAndSubCategory.Key == DefaultFloorLampConfig.DEFAULT_ID)
						{
							var defaultStainedDef = Assets.GetBuildingDef(DefaultFloorLampConfig.DEFAULT_ID);

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
