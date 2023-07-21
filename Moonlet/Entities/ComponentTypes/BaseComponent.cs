using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public abstract class BaseComponent
	{
		public abstract void Apply(GameObject prefab);

		public virtual void OnConfigureBuildingPreview(GameObject prefab)
		{
		}

		public virtual void OnConfigureBuildingUnderConstruction(GameObject prefab)
		{
		}

		public virtual void OnConfigureBuildingTemplate(GameObject prefab)
		{
		}

		public virtual void OnConfigureBuildingComplete(GameObject prefab)
		{
		}
	}
}
