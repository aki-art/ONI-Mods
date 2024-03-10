namespace Twitchery.Content.Scripts
{
	public class AETE_ModCleanup : KMonoBehaviour
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			Mod.cleanup.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.cleanup.Remove(this);
		}

		public void OnModCleanup()
		{
			foreach (var component in this.GetComponents<IOnModCleanup>())
			{
				component.OnModCleanup();
			}
		}
	}
}
