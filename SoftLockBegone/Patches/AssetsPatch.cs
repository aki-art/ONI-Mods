using HarmonyLib;
using SoftLockBegone;

namespace SoftLockBegone.Patches
{
	public class AssetsPatch
	{

		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				ModAssets.InitTemplate();
			}
		}
	}
}
/*
		[HarmonyPatch(typeof(Assets), "GetDef")]
		public class Assets_GetBuildingDef_Patch
		{
			[HarmonyPriority(Priority.Last)]
			public static void Postfix(IList<BuildingDef> defs, string prefab_id, ref BuildingDef __result)
			{
				if (__result != null)
					return;

				var config = new PlaceHolderBuildingConfig()
				{
					skipLoading = false,
					ID = prefab_id,
				};

				var def = config.CreateBuildingDef();

				RegisterBuilding(config, def);

				__result = def;
			}
		}

		public static void RegisterBuilding(IBuildingConfig config, BuildingDef buildingDef)
		{
			buildingDef.RequiredDlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			BuildingConfigManager.Instance.configTable[config] = buildingDef;

			var gameObject = Object.Instantiate(BuildingConfigManager.Instance.baseTemplate);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
			gameObject.name = buildingDef.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = buildingDef;
			gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);

			// OnConfigureTemplate

			buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);

			buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
			buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
			buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
			buildingDef.BuildingPreview.name += "Preview";

			buildingDef.PostProcess();

			//data.Components?.Do(cmp => cmp.OnConfigureBuildingComplete(buildingDef.BuildingComplete));
			//data.Components?.Do(cmp => cmp.OnConfigureBuildingPreview(buildingDef.BuildingPreview));
			//data.Components?.Do(cmp => cmp.OnConfigureBuildingUnderConstruction(buildingDef.BuildingUnderConstruction));

			Assets.AddBuildingDef(buildingDef);
		}

		//[HarmonyPatch(typeof(Assets), "TryGetPrefab")]
		public class Assets_TryGetPrefab_Patch
		{
			public static void Postfix(Tag tag, ref GameObject __result)
			{
				if (__result == null)
				{
					Log.Warning($"Missing prefab: {tag}. Replacing with placeholder.");

					var prefab = Object.Instantiate(ModAssets.placeholderBuildingTemplate);
					var kPrefabID = prefab.GetComponent<KPrefabID>();

					var kbac = prefab.GetComponent<KBatchedAnimController>();
					kbac.animFiles = new[] { Assets.GetAnim("barbeque_kanim") };

					kPrefabID.PrefabTag = tag;
					kPrefabID.prefabSpawnFn += go =>
					{
						go.GetComponent<KSelectable>().SetName(tag.Name);
						var kbac = go.GetComponent<KBatchedAnimController>();
						var occupy = go.GetComponent<OccupyArea>();
						kbac.animHeight *= occupy.GetHeightInCells();
						kbac.animWidth *= occupy.GetWidthInCells();
					};

					__result = prefab;
					Assets.Prefabs.Add(kPrefabID);
					Assets.PrefabsByTag.Add(tag, kPrefabID);
				}
			}
		}
	}
}
*/