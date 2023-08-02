using Moonlet.Content.Scripts;
using System.Linq;
using UnityEngine;

namespace Moonlet.Entities.Commands
{
	internal class RandomAnimationCommand : BaseCommand
	{
		public const string PERSISTENT_ANIMATION_KEY = "persistentAnimation";
		public const string PERSISTENT_SCALE_KEY = "persistentScale";

		public string[] Options { get; set; }

		public bool Persistent { get; set; }

		public KAnim.PlayMode Mode { get; set; }

		public float? MinScale { get; set; }

		public float? MaxScale { get; set; }

		public override void Run(GameObject go)
		{

			if (go.TryGetComponent(out MoonletEntityComponent entity)
				&& go.TryGetComponent(out KBatchedAnimController kbac))
			{
				if (entity.serializedData.TryGetValue(PERSISTENT_ANIMATION_KEY, out var data))
				{
					if (data is string persistentAnim && Options.Contains(persistentAnim))
					{
						kbac.Play(persistentAnim, Mode);

						if (entity.serializedData.TryGetValue(PERSISTENT_SCALE_KEY, out var scale) && scale is float scaleValue)
							kbac.animScale *= scaleValue;
						else
							SetRandomScale(entity, kbac);

						return;
					}
				}

				SetRandomAnim(entity, kbac);
				SetRandomScale(entity, kbac);
			}
		}

		private void SetRandomAnim(MoonletEntityComponent entity, KBatchedAnimController kbac)
		{
			var anim = Options.GetRandom();
			entity.serializedData[PERSISTENT_ANIMATION_KEY] = anim;
			kbac.Play(anim, Mode);
		}

		private void SetRandomScale(MoonletEntityComponent entity, KBatchedAnimController kbac)
		{
			var minScale = MinScale ?? 1;
			var maxScale = MaxScale ?? 1;

			var scaleValue = Random.Range(minScale, maxScale);
			kbac.animScale *= scaleValue;
			entity.serializedData[PERSISTENT_SCALE_KEY] = scaleValue;
		}
	}
}
