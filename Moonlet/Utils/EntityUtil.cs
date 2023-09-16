using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Utils
{
	public class EntityUtil
	{
		public static Dictionary<string, Type> mappings = new()
		{
			// components
/*			{ "tag:yaml.org,2002:sublimates", typeof(SublimatesComponent) },
			{ "tag:yaml.org,2002:edible", typeof(EdibleComponent) },
			{ "tag:yaml.org,2002:lightEmitter", typeof(LightEmitterComponent) },
			{ "tag:yaml.org,2002:radiationEmitter", typeof(RadiationEmitterComponent) },
			{ "tag:yaml.org,2002:storage", typeof(StorageComponent) },
			{ "tag:yaml.org,2002:demolishable", typeof(DemolishableComponent) },
			{ "tag:yaml.org,2002:rummagable", typeof(RummagableComponent) },
			{ "tag:yaml.org,2002:simpleGenerator", typeof(SimpleGeneratorComponent) },
			{ "tag:yaml.org,2002:diggable", typeof(DiggableComponent) },
			{ "tag:yaml.org,2002:foundationMonitor", typeof(FoundationMonitorComponent) },
			{ "tag:yaml.org,2002:fabricator", typeof(FabricatorComponent) },

			// commands
			{ "tag:yaml.org,2002:destroy", typeof(DestroyCommand) },
			{ "tag:yaml.org,2002:spawnitems", typeof(SpawnItemsCommand) },
			{ "tag:yaml.org,2002:spawnelement", typeof(SpawnElementCommand) },
			{ "tag:yaml.org,2002:playRandom", typeof(RandomAnimationCommand) },
			{ "tag:yaml.org,2002:play", typeof(PlayAnimationCommand) },
			{ "tag:yaml.org,2002:dropMaterials", typeof(DropMaterialsCommand) },*/
		};


/*		public static void ProcessCommands(EntityData data, GameObject prefab)
		{
			if (prefab.TryGetComponent(out KPrefabID prefabID))
			{
				if (data.OnSpawn != null)
				{
					foreach (var item in data.OnSpawn)
					{
						if (item == null)
							continue;

						prefabID.prefabSpawnFn += item.Run;
					}
				}

				// have to do this an odd ways because Unity is unwilling to serialize Lists or Events in mods for some reason
				if (data.OnDestroy != null)
				{
					foreach (var item in data.OnDestroy)
					{
						if (item == null)
							continue;

						prefabID.prefabInitFn += go =>
						{
							if (go.TryGetComponent(out MoonletEntityComponent component))
								component.AddOnDestroyFn(item.Run);
						};
					}
				}
			}
		}

		public static void ProcessComponents(EntityData data, GameObject prefab)
		{
			var components = data.Components;

			if (components == null)
				return;

			foreach (var component in components)
			{
				if (component == null)
				{
					FUtility.Log.Debuglog("null component");
					continue;
				}

				FUtility.Log.Debuglog("processing component: " + component.GetType());
				component?.Apply(prefab);
			}
		}*/
	}
}
