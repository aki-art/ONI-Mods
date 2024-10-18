using Moonlet.Scripts.ComponentTypes;
using Moonlet.Templates;
using System.Linq;
using UnityEngine;

namespace Moonlet.Scripts
{
	namespace Moonlet.Entities
	{
		public class GenericBuildingConfig : IBuildingConfig
		{
			public BuildingDef def;
			public BuildingTemplate template;
			public bool skipLoading = true; // used to skip the default unconfigured configs

			public override BuildingDef CreateBuildingDef() => ConfigureDef(def);

			public GenericBuildingConfig()
			{

			}

			public GenericBuildingConfig(bool skipLoading, BuildingTemplate template)
			{
				this.template = template;
				this.skipLoading = skipLoading;
				def = CreateBasicDef();
			}

			public virtual BuildingDef ConfigureDef(BuildingDef def) => def;

			public virtual BuildingDef CreateBasicDef()
			{
				if (template.Materials == null)
				{
					Log.Error($"Buildings require at least 1 material.");
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
					template.Decor == null ? TUNING.DECOR.NONE : template.Decor.Get(),
					default);

				def.AudioCategory = template.AudioCategory;
				def.AudioSize = template.AudioSize;

				if (!template.OverlayMode.IsNullOrWhiteSpace())
					def.ViewMode = template.OverlayMode;
				else if (template.Components != null && template.Components.Any(comp => comp is LightEmitterComponent))
					def.ViewMode = OverlayModes.Light.ID;
				else if (template.PowerInlet != null || template.PowerOutlet != null)
					def.ViewMode = OverlayModes.Power.ID;
				else if (template.Tags != null && template.Tags.Contains(GameTags.Decoration.ToString()))
					def.ViewMode = OverlayModes.Decor.ID;

				ConfigureConduits(def);
				ConfigurePower(def);

				return def;
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
				def.EnergyConsumptionWhenActive = template.EnergyConsumptionWhenActive;

			}

			public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
			{
				base.ConfigureBuildingTemplate(go, prefab_tag);
			}

			public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
			{
				base.DoPostConfigurePreview(def, go);
			}

			public override void DoPostConfigureUnderConstruction(GameObject go)
			{
				base.DoPostConfigureUnderConstruction(go);
			}

			public override void DoPostConfigureComplete(GameObject go)
			{
			}
		}
	}
}
