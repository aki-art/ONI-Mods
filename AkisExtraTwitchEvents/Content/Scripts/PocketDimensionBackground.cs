using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class PocketDimensionBackground : KMonoBehaviour
	{
		[SerializeField] public string animFile;
		[SerializeField] public string animName;
		[SerializeField] public float scale;

		[MyCmpReq] private KBatchedAnimController animController;

		public override void OnSpawn()
		{
			base.OnSpawn();

			animController.SwapAnims(new[]
			{
				Assets.GetAnim(animFile),
			});

			animController.Play(animName);
			animController.animScale *= scale;

			Subscribe((int)GameHashes.WorldRemoved, OnWorldRemoved);
		}

		private void OnWorldRemoved(object _)
		{
			Util.KDestroyGameObject(gameObject);
		}
	}
}
