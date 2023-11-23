using KSerialization;

namespace DecorPackB.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DecorPackB_Mod : KMonoBehaviour
	{
		public static DecorPackB_Mod Instance;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.Loaded, OnLoaded);
		}

		private void OnLoaded(object obj)
		{
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}
	}
}
