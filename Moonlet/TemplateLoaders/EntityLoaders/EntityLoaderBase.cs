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

			var holder = prefab.AddComponent<MoonletComponentHolder>();
			holder.sourceMod = sourceMod;
			holder.addToSandboxMenu = template.AddToSandboxMenu;

			ProcessComponents(prefab);
			ProcessCommands(holder);


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

			ConfigureKbac(prefab);

			if (template.Layers != null)
			{
				var occupyArea = prefab.AddComponent<OccupyArea>();
				occupyArea.objectLayers = template.Layers;
				occupyArea.SetCellOffsets(FUtility.Utils.MakeCellOffsets((int)template.Width, (int)template.Height));
			}

			//prefab.AddOrGet<LoopingSounds>();

			return prefab;
		}

		protected void ConfigureKbac(GameObject prefab)
		{
			if (prefab.TryGetComponent(out KBatchedAnimController kbac))
			{
				var mode = EnumUtils.ParseOrDefault(template.Animation?.PlayMode, KAnim.PlayMode.Once);
				kbac.initialMode = mode;
			}
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

		protected void ProcessCommands(MoonletComponentHolder componentHolder)
		{
			var commands = template.Commands;
			if (commands == null) return;

			foreach (var command in commands)
			{
				if (command != null && command.CanApplyTo(componentHolder.gameObject))
					componentHolder.GetComponent<KPrefabID>().prefabSpawnFn += go =>
					{
						go.GetComponent<MoonletComponentHolder>().OnPrefabSpawn(command);
					};
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
