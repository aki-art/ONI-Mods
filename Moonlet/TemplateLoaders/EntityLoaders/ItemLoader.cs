using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class ItemLoader(ItemTemplate template, string sourceMod) : EntityLoaderBase<ItemTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.ITEMS.PREFABS.{id.ToUpperInvariant()}.{partialKey}";

		public override void LoadContent()
		{
			var anim = Assets.GetAnim(template.Animation?.File);

			var prefab = EntityTemplates.CreateLooseEntity(
				template.Id,
				template.Name,
				template.Description,
				template.Mass,
				true,
				anim,
				template.Animation?.DefaultAnimation == null ? "object" : template.Animation.DefaultAnimation,
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.CIRCLE,
				template.Width,
				template.Height,
				true,
				0,
				ElementUtil.GetSimhashSafe(template.Element),
				template.Tags?.ToTagList()); ;

			//prefab.AddOrGet<MoonletEntityComponent>();

			//EntityUtil.ProcessCommands(template, prefab);
			//EntityUtil.ProcessComponents(template, prefab);

			Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}
	}
}
