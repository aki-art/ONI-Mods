using Moonlet.Templates.EntityTemplates;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public abstract class EntityLoaderBase<EntityType>(EntityType template, string sourceMod)
		: TemplateLoaderBase<EntityType>(template, sourceMod), ILoadableContent
		where EntityType : EntityTemplate
	{
		public abstract void LoadContent();
	}
}
