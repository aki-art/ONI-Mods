using FUtility;
using HarmonyLib;
using SoftLockBegone;
using SoftLockBegone.Content;
using SoftLockBegone.Content.Scripts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TUNING;
using UnityEngine;

namespace SoftLockBegone.Patches
{
	internal class SaveManagerPatch
	{
		public static void RegisterBuilding(IBuildingConfig config, BuildingDef buildingDef)
		{
			buildingDef.RequiredDlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			BuildingConfigManager.Instance.configTable[config] = buildingDef;

			var gameObject = Object.Instantiate(BuildingConfigManager.Instance.baseTemplate);
			Object.DontDestroyOnLoad(gameObject);
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


		[HarmonyPatch(typeof(SaveManager), "Load", typeof(IReader))]
		public class SaveManager_Load_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var m_SkipBytes = AccessTools.Method(typeof(IReader), nameof(IReader.SkipBytes));

				// find injection point
				var index = codes.FindIndex(ci => ci.Calls(m_SkipBytes));

				if (index == -1)
				{
					Log.Warning("No skipbytes found, cannot restore prefabs.");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(SaveManager_Load_Patch), nameof(InjectedMethod));

				// remove yeeting the data

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					// ireader on stack
					// int on stack
					new CodeInstruction(OpCodes.Ldloc, 7),
					new CodeInstruction(OpCodes.Ldloc, 6),
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Callvirt, m_InjectedMethod)
				});

				codes.RemoveAt(index);

				return codes;
			}

			private static bool logged = false;
			private static void InjectedMethod(IReader reader, int length, int capacity, string tag, SaveManager instance)
			{
				var saveLoadRootList = new List<SaveLoadRoot>(capacity);
				instance.sceneObjects[tag] = saveLoadRootList;

				if(!logged)
				{
					foreach (var tpe in KSerialization.Manager.deserializationTemplatesByType)
					{
						Log.Debuglog($"\t - {tpe.Key} {tpe.Value}");
					}
				}

				for (int index2 = 0; index2 < capacity; ++index2)
				{
					var data = reader.ReadBytes(length);
					(reader as FastReader).Position -= length; // reset

					var prefab = Assets.GetPrefab(SLBEntityConfig.ID);

					instance.prefabMap[tag] = prefab;

					var saveLoadRoot = SaveLoadRoot.Load(prefab, reader);
					if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && saveLoadRoot == null)
					{
						Debug.LogError("Error loading data [" + tag + "]");
						return;
					}

					saveLoadRoot.GetComponent<SLB_EntityComponent>().SetData(tag, data, capacity);
				}

				Log.Info("Rescued data of missing prefab: " + tag);
			}
		}
	}
}
