using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class FrozenEntityContainer : MidasEntityContainer, ISim1000ms
	{
		[MyCmpReq] private PrimaryElement primaryElement;

		private static Color blue = Util.ColorFromHex("52bcff");

		public override Color GetOverlayColor() => blue;

		public override StatusItem GetStatusItem() => TStatusItems.FrozenStatus;

		public override void OnCleanUp()
		{
			Release(true);
			base.OnCleanUp();
			Mod.midasContainers.Remove(this);
		}

		public void Sim1000ms(float dt)
		{
			if(primaryElement.Temperature > 273.15)
			{
				Release(true);
				Util.KDestroyGameObject(gameObject);
			}
		}
	}
}
