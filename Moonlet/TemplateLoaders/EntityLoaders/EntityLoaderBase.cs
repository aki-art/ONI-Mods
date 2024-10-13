using Moonlet.Scripts;
using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public abstract class EntityLoaderBase<EntityType>(EntityType template, string sourceMod)
		: TemplateLoaderBase<EntityType>(template, sourceMod), ILoadableContent
		where EntityType : EntityTemplate
	{
		public virtual void LoadContent()
		{
			var prefab = CreatePrefab();

			prefab.AddComponent<MoonletComponentHolder>().addToSandboxMenu = template.AddToSandboxMenu;

			ProcessComponents(prefab);

			Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
		}

		protected GameObject CreateBasicPlacedEntity()
		{
			var elementId = ElementUtil.GetSimhashSafe(template.Element, SimHashes.Creature);

			var prefab = EntityTemplates.CreatePlacedEntity(
				template.Id,
				template.Name,
				template.Description,
				template.Mass,
				Assets.GetAnim(template.Animation.GetFile()),
				template.Animation.DefaultAnimation,
				Grid.SceneLayer.Creatures,
				(int)template.Width,
				(int)template.Height,
				template.Decor == null ? default : template.Decor.Get(),
				default,
				elementId,
				template.Tags?.ToTagList(),
				template.DefaultTemperature.CalculateOrDefault(288.05f));

			if (template.Layers != null)
			{
				var occupyArea = prefab.AddComponent<OccupyArea>();
				occupyArea.objectLayers = template.Layers;
				occupyArea.SetCellOffsets(FUtility.Utils.MakeCellOffsets((int)template.Width, (int)template.Height));
			}

			return prefab;
		}

		protected abstract GameObject CreatePrefab();

		protected void ProcessComponents(GameObject prefab)
		{
			var components = template.Components;

			if (components == null)
				return;

			foreach (var component in components)
			{
				if (component != null && component.CanApplyTo(prefab))
					component.Apply(prefab);
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			id = template.Id;
		}

		public override void Validate()
		{
			if (Mathf.Approximately(template.Mass, 0))
				template.Mass = 10;
		}
	}
}
