using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class RandomizeAnimationComponent : BaseComponent
	{
		public const string dataKey = "RandomizedAnimation";

		public RandomAnimationData Data { get; private set; }

		public class RandomAnimationData
		{
			public string[] Options { get; set; }

			public string Mode { get; set; }
		}

		public override bool CanApplyTo(GameObject prefab)
		{
			return prefab.GetComponent<KBatchedAnimController>() != null;
		}

		public override void Apply(GameObject prefab)
		{
			if (!CheckData(Data))
				return;

			if (!prefab.TryGetComponent(out KBatchedAnimController kbac))
				return;

			if (Data.Options == null || Data.Options.Length == 0)
			{
				Log.Warn("Cannot use RandomAnimationComponent with no options defined.");
				return;
			}

			if (prefab.TryGetComponent(out MoonletComponentHolder holder))
			{
				var initialMode = EnumUtils.ParseOrDefault(Data.Mode, KAnim.PlayMode.Paused);

				Log.Debug("adding items " + Data.Options.Length);
				holder.GetComponent<KPrefabID>().prefabSpawnFn += go =>
				{
					Log.Debug("onprefab spawn " + Data.Options.Length);
					holder.initialAnimOverrideOptions = Data.Options;
					holder.initialModeOverride = initialMode;
				};
			}
		}
	}
}
