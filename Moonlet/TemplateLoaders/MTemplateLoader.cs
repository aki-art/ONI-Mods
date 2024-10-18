using Moonlet.Templates;
using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using TemplateClasses;

namespace Moonlet.TemplateLoaders
{
	public class MTemplateLoader(TemplateTemplate template, string sourceMod) : TemplateLoaderBase<TemplateTemplate>(template, sourceMod)
	{
		public override void Initialize()
		{
			id = $"{relativePath}";
			if (id.StartsWith("/") || id.StartsWith("\\"))
			{
				id = id.Substring(1);
			}

			template.Id = id;

			base.Initialize();
		}

		public TemplateContainer GetOrLoad()
		{
			if (TemplateCache.templates.TryGetValue(template.Id, out var existing))
				return existing;

			var result = Get();
			TemplateCache.templates[template.Id] = result;

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

			return result;
		}

		public override void RegisterTranslations()
		{
		}
	}
}
