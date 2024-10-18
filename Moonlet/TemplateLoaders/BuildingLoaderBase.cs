using HarmonyLib;
using Moonlet.TemplateLoaders.EntityLoaders;
using Moonlet.Templates;
using TUNING;
using UnityEngine;
using KBuildingLoader = global::BuildingLoader;

namespace Moonlet.TemplateLoaders
{
	public abstract class BuildingLoaderBase<TemplateType>(TemplateType template, string sourceMod) : EntityLoaderBase<TemplateType>(template, sourceMod) where TemplateType : BuildingTemplate
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.BUILDINGS.PREFABS.{template.Id.ToUpperInvariant()}.{partialKey}";

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
			AddString(GetTranslationKey("EFFECT"), template.EffectDescription);
		}

		public virtual void CreateAndRegister()
		{
			var config = CreateConfig();
			var def = config.CreateBuildingDef();

			RegisterBuilding(config, def, template);
			AddToTech();
			AddToMenu();
		}

		public abstract IBuildingConfig CreateConfig();

		protected void AddToTech()
		{
			if (!template.ResearchCategory.IsNullOrWhiteSpace())
				FUtility.BuildingUtil.AddToResearch(template.Id, template.ResearchCategory);
		}

		protected void AddToMenu()
		{
			if (!template.Category.IsNullOrWhiteSpace())
			{
				string neighbor = template.After;
				if (!template.Before.IsNullOrWhiteSpace())
				{
					if (!template.After.IsNullOrWhiteSpace())
						Warn("A building cannot be both before and after another building.");
					else
						neighbor = template.Before;
				}

				var ordering = !template.Before.IsNullOrWhiteSpace() ? ModUtil.BuildingOrdering.Before : ModUtil.BuildingOrdering.After;
				ModUtil.AddBuildingToPlanScreen(template.Category, template.Id, template.SubCategory, neighbor, ordering);
			}
		}

		protected void RegisterBuilding(IBuildingConfig config, BuildingDef buildingDef, BuildingTemplate data)
		{
			if (!DlcManager.IsDlcListValidForCurrentContent(data.DlcIds))
				return;

			buildingDef.RequiredDlcIds = data.DlcIds;
			BuildingConfigManager.Instance.configTable[config] = buildingDef;

			var gameObject = Object.Instantiate(BuildingConfigManager.Instance.baseTemplate);
			Object.DontDestroyOnLoad(gameObject);
			gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
			gameObject.name = buildingDef.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = buildingDef;
			gameObject.AddTag(GameTags.RoomProberBuilding);
			gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);

			config.ConfigureBuildingTemplate(gameObject, gameObject.PrefabID());
			data.Components?.Do(cmp => cmp.OnConfigureBuildingTemplate(gameObject));

			if (data.PowerConsumption.HasValue)
			{
				gameObject.AddOrGetDef<PoweredController.Def>();
				gameObject.AddOrGet<EnergyConsumer>();
			}

			if (data.Prioritizable)
				Prioritizable.AddRef(gameObject);

			buildingDef.BuildingComplete = KBuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);

			if (!data.DisallowBuildingByPlayer)
			{
				buildingDef.BuildingUnderConstruction = KBuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
				buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
				buildingDef.BuildingPreview = KBuildingLoader.Instance.CreateBuildingPreview(buildingDef);
				buildingDef.BuildingPreview.name += "Preview";
			}

			ConfigureKbac(buildingDef.BuildingComplete);

			config.DoPostConfigureComplete(buildingDef.BuildingComplete);
			data.Components?.Do(cmp => cmp.OnConfigureBuildingComplete(buildingDef.BuildingComplete));

			if (buildingDef.BaseDecor > BUILDINGS.DECOR.BONUS.TIER3.amount)
				buildingDef.BuildingComplete.AddTag(RoomConstraints.ConstraintTags.Decor20);

			buildingDef.PostProcess();

			if (!data.DisallowBuildingByPlayer)
			{
				config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
				data.Components?.Do(cmp => cmp.OnConfigureBuildingPreview(buildingDef.BuildingPreview));
				config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
				data.Components?.Do(cmp => cmp.OnConfigureBuildingUnderConstruction(buildingDef.BuildingUnderConstruction));
			}

			Assets.AddBuildingDef(buildingDef);
		}

		protected override GameObject CreatePrefab() => throw new System.NotImplementedException();
	}
}
