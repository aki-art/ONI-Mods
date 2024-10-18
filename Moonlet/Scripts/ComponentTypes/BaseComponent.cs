using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public abstract class BaseComponent
	{
		public string Type { get; set; } // for YAML type detection

		public bool Optional { get; set; }

		public BaseComponent()
		{
			Validate();
		}

		protected virtual void Validate()
		{
			if (GetType().GetProperty("Data") == null)
				Log.Warn($"Component {GetType().Name} does not have a Data property defined.");
		}

		protected virtual bool CheckData(object data)
		{
			if (data == null)
			{
				Log.Warn(GetType().Name + " must have Data defined!");
				return false;
			}

			return true;
		}

		public abstract void Apply(GameObject prefab);

		public virtual bool CanApplyTo(GameObject prefab) => true;

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
