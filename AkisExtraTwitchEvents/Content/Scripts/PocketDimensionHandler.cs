namespace Twitchery.Content.Scripts
{
	public class PocketDimensionHandler : KMonoBehaviour
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			ClusterManager.Instance.Subscribe((int)GameHashes.WorldRemoved, OnWorldRemoved);
		}

		private void OnWorldRemoved(object data)
		{
			if (data is int worldId && this.GetMyWorldId() == worldId)
				Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			ClusterManager.Instance.Unsubscribe((int)GameHashes.WorldRemoved, OnWorldRemoved);
		}
	}
}
