using KSerialization;

namespace DecorPackB.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DecorPackB_Mod : KMonoBehaviour
	{
		public static DecorPackB_Mod Instance;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.Loaded, OnLoaded);
		}

		private void OnLoaded(object obj)
		{
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}
	}
}
