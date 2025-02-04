using HarmonyLib;
using Moonlet.Templates;
using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System.Collections.Generic;
using System.Linq;
using TemplateClasses;
using static ProcGen.SubWorld;

namespace Moonlet.TemplateLoaders
{
	public class MTemplateLoader(TemplateTemplate template, string sourceMod) : TemplateLoaderBase<TemplateTemplate>(template, sourceMod)
	{
		public const string LOOKUP_PREFIX = "Moonlet_Lookup_";

		public List<ZoneTypeOverride> zoneTypeOverrides;

		public ZoneType zone;

		public class ZoneTypeOverride
		{
			public int x;
			public int y;
			public ZoneType zone;
		}

		public override void Initialize()
		{
			id = $"{relativePath.TrimStart('/')}";

			template.Id = id;

			base.Initialize();
		}

		public TemplateContainer GetOrLoad()
		{
			TemplateCache.Init();

			if (TemplateCache.templates.TryGetValue(template.Id, out var existing))
				return existing;

			var result = Get();

			TemplateCache.templates[template.Id] = result;

			if (template.UniformZoneType != null && template.Cells != null)
			{
				template.ZoneTypes ??= [];
				foreach (var cell in template.Cells)
				{
					if (!template.ZoneTypes.Any(z => z != null
						&& cell.LocationX != null
						&& cell.LocationY != null
						&& z.LocationX == cell.LocationX
						&& z.LocationY == cell.LocationY))
						template.ZoneTypes.Add(new MTemplateZoneType()
						{
							LocationX = cell.LocationX,
							LocationY = cell.LocationY,
							ZoneType = template.UniformZoneType
						});
				}
			}

			if (template.ZoneTypes != null)
			{
				zoneTypeOverrides = [];
				for (var i = template.ZoneTypes.Count - 1; i >= 0; i--)
				{
					var zoneType = template.ZoneTypes[i];
					if (EnumUtils.TryParse(zoneType.ZoneType, out ZoneType zone, ZoneTypeUtil.quickLookup))
					{
						zoneTypeOverrides.Add(new ZoneTypeOverride()
						{
							zone = zone,
							x = zoneType.LocationX.CalculateOrDefault(0),
							y = zoneType.LocationY.CalculateOrDefault(0),
						});
					}
				}
			}

			return result;
		}

		public TemplateContainer Get()
		{
			var result = CopyProperties<TemplateContainer>();
			result.info = template.Info.Convert();
			result.name = template.Id;
			result.buildings = ShadowTypeUtil.CopyList<Prefab, MTemplatePrefab>(template.Buildings, Issue);
			result.otherEntities = ShadowTypeUtil.CopyList<Prefab, MTemplatePrefab>(template.OtherEntities, Issue);
			result.pickupables = ShadowTypeUtil.CopyList<Prefab, MTemplatePrefab>(template.Pickupables, Issue);
			result.elementalOres = ShadowTypeUtil.CopyList<Prefab, MTemplatePrefab>(template.ElementalOres, Issue);
			result.cells = ShadowTypeUtil.CopyList<Cell, MTemplateCell>(template.Cells, Issue) ?? [];

			result.info.tags ??= [];
			result.info.tags = result.info.tags.AddToArray(LOOKUP_PREFIX + id);

			if (template.Cells != null)
				foreach (var element in template.Cells)
					Mod.elementsLoader.RequestElement(element.Element, sourceMod);

			if (template.ElementalOres != null)
				foreach (var element in template.ElementalOres)
					Mod.elementsLoader.RequestElement(element.Element, sourceMod);

			if (template.Buildings != null)
				foreach (var element in template.Buildings)
					Mod.elementsLoader.RequestElement(element.Element, sourceMod);

			if (template.OtherEntities != null)
				foreach (var element in template.OtherEntities)
					Mod.elementsLoader.RequestElement(element.Element, sourceMod);

			if (template.Pickupables != null)
				foreach (var element in template.Pickupables)
					Mod.elementsLoader.RequestElement(element.Element, sourceMod);

			return result;
		}

		public override void RegisterTranslations()
		{
		}
	}
}
