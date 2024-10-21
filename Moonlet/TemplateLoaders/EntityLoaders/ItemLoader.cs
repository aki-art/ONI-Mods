using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class ItemLoader(ItemTemplate template, string sourceMod) : EntityLoaderBase<ItemTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.ITEMS.PREFABS.{id.ToUpperInvariant()}.{partialKey}";

		protected override GameObject CreatePrefab()
		{
			var anim = Assets.GetAnim(template.Animation?.GetFile());

			var prefab = EntityTemplates.CreateLooseEntity(
				template.Id,
				template.Name,
				template.Description,
				template.Mass,
				true,
				anim,
				template.Animation?.DefaultAnimation == null ? "object" : template.Animation.DefaultAnimation,
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				template.Width,
				template.Height,
				true,
				0,
				ElementUtil.GetSimhashIfLoadedOrDefault(template.Element, SimHashes.Creature),
				template.Tags?.ToTagList());

			return prefab;
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}
	}
}
