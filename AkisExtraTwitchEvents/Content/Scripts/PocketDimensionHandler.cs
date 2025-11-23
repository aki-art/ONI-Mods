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
			var worldId = Boxed<int>.Unbox(data);
			if (this.GetMyWorldId() == worldId)
				Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			ClusterManager.Instance.Unsubscribe((int)GameHashes.WorldRemoved, OnWorldRemoved);
		}
	}
}
