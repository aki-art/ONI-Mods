using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class DebrisLoader(ItemTemplate template, string sourceMod) : EntityLoaderBase<ItemTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.ITEMS.PREFABS.{id.ToUpperInvariant()}.{partialKey}";

		protected override GameObject CreatePrefab()
		{
			var anim = Assets.GetAnim(template.Animation?.File);

			GameObject prefab = null;

			var element = global::ElementLoader.FindElementByName(template.Id);

			if (element == null)
			{
				Warn($"No element with id {template.Id}");
				return null;
			}

			var tags = template.Tags?.ToTagList();

			if (element.IsSolid)
				prefab = EntityTemplates.CreateSolidOreEntity(element.id, tags);
			else if (element.IsLiquid)
				prefab = EntityTemplates.CreateLiquidOreEntity(element.id, tags);
			else if (element.IsGas)
				prefab = EntityTemplates.CreateGasOreEntity(element.id, tags);

			ProcessComponents(prefab);
			//ProcessCommands(debris, prefab);

			Assets.AddPrefab(prefab.GetComponent<KPrefabID>());

			return prefab;
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}

		internal void LoadContent(HashSet<SimHashes> simHashes)
		{
			var simHash = ElementUtil.GetSimhashSafe(template.Id);
			if (simHash == SimHashes.Void)
			{
				Warn($"Could not load {template.Id}, there is no element registered with this ID.");
				return;
			}

			base.LoadContent();
			simHashes.Add(simHash);
		}
	}
}
