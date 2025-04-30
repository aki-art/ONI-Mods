using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class TinyCrab : KMonoBehaviour
	{
		private static readonly Color overlayColor = Util.ColorFromHex("c24f5f");

		public override void OnSpawn()
		{
			base.OnSpawn();
			var kbac = GetComponent<KBatchedAnimController>();
			kbac.animScale *= 0.33f;
			kbac.TintColour = overlayColor;
		}
	}
}
