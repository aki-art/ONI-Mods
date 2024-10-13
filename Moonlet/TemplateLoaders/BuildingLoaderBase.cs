using HarmonyLib;
using Moonlet.Scripts.Moonlet.Entities;
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

		public abstract void CreateAndRegister();

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

		protected void ConfigureConduits(BuildingDef def)
		{
			if (template.ConduitIn != null)
			{
				def.InputConduitType = template.ConduitIn.Type;
				def.UtilityInputOffset = new(template.ConduitIn.X, template.ConduitIn.Y);
			}

			if (template.ConduitOut != null)
			{
				def.OutputConduitType = template.ConduitIn.Type;
				def.UtilityOutputOffset = new(template.ConduitOut.X, template.ConduitOut.Y);
			}
		}

		protected void ConfigurePower(BuildingDef def)
		{
			if (template.PowerOutlet != null)
			{
				def.RequiresPowerOutput = true;
				def.PowerOutputOffset = new(template.PowerOutlet.X, template.PowerOutlet.Y);
			}

			if (template.PowerInlet != null)
			{
				def.RequiresPowerInput = true;
				def.PowerInputOffset = new(template.PowerInlet.X, template.PowerInlet.Y);
			}

			if (template.Generator != null)
			{
				def.GeneratorBaseCapacity = template.Generator.BaseCapacity;
				def.GeneratorWattageRating = template.Generator.Wattage;
			}

			if (template.PowerConsumption.HasValue)
			{
				def.RequiresPowerInput = true;
				def.EnergyConsumptionWhenActive = template.PowerConsumption.Value;
			}

			def.ExhaustKilowattsWhenActive = template.ExhaustKilowattsWhenActive;
			def.SelfHeatKilowattsWhenActive = template.SelfHeatKilowattsWhenActive;

		}

		protected virtual BuildingDef ConfigureDef()
		{
			if (template.Materials == null)
			{
				Error($"Buildings require at least 1 material.");
				return null;
			}

			var mass = new float[template.Materials.Length];
			var ingredients = new string[template.Materials.Length];

			for (int i = 0; i < template.Materials.Length; i++)
			{
				var material = template.Materials[i];
				mass[i] = material.Mass;
				ingredients[i] = material.Material;
			}

			var def = BuildingTemplates.CreateBuildingDef(
				template.Id,
				(int)template.Width,
				(int)template.Height,
				template.Animation.GetFile(),
				template.HitPoints,
				template.ConstructionTime,
				mass,
				ingredients,
				template.MeltingPointKelvin,
				template.BuildLocationRule,
				template.Decor == null ? DECOR.NONE : template.Decor.Get(),
				default);

			def.AudioCategory = template.AudioCategory;
			def.AudioSize = template.AudioSize;

			return def;
		}

		protected void RegisterBuilding(GenericBuildingConfig config, BuildingDef buildingDef, BuildingTemplate data)
		{
			Debug("registering " + id);
			if (!DlcManager.IsDlcListValidForCurrentContent(data.DlcIds))
				return;

			Debug(1);
			buildingDef.RequiredDlcIds = data.DlcIds;
			BuildingConfigManager.Instance.configTable[config] = buildingDef;

			var gameObject = Object.Instantiate(BuildingConfigManager.Instance.baseTemplate);
			Object.DontDestroyOnLoad(gameObject);
			gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
			gameObject.name = buildingDef.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = buildingDef;
			gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);

			data.Components?.Do(cmp => cmp.OnConfigureBuildingTemplate(gameObject));

			if (data.PowerConsumption.HasValue)
				gameObject.AddOrGetDef<PoweredController.Def>();

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

			buildingDef.PostProcess();

			if (!data.DisallowBuildingByPlayer)
			{
				data.Components?.Do(cmp => cmp.OnConfigureBuildingPreview(buildingDef.BuildingPreview));
				data.Components?.Do(cmp => cmp.OnConfigureBuildingUnderConstruction(buildingDef.BuildingUnderConstruction));
			}

			Debug("added to assets");
			Assets.AddBuildingDef(buildingDef);
		}

		protected override GameObject CreatePrefab() => throw new System.NotImplementedException();
	}
}
