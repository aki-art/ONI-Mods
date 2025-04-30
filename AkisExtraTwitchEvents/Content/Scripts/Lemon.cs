using KSerialization;
using System.Collections.Generic;

namespace Twitchery.Content.Scripts
{
	public class Lemon : KMonoBehaviour
	{
		[Serialize] public string animation;

		private static readonly List<string> anims =
			["object", "object1", "object3"];

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (animation.IsNullOrWhiteSpace())
				animation = anims.GetRandom();

			GetComponent<KBatchedAnimController>().Play(animation);
		}
	}
}
