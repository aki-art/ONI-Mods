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

		public void LoadContent()
		{
			TemplateCache.templates[template.Id] = Get();
			Log.Debug("registered template: " + template.Id);
			Log.Debug(TemplateCache.templates[template.Id] != null);
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

			return result;
		}

		public override void RegisterTranslations()
		{
		}
	}
}
