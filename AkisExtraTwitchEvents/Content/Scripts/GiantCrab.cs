namespace Twitchery.Content.Scripts
{
	public class GiantCrab : KMonoBehaviour
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.giantCrabs.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.giantCrabs.Remove(this);
		}
	}
}
