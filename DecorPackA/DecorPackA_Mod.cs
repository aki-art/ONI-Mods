using KSerialization;

namespace DecorPackA
{
	public class DecorPackA_Mod : KMonoBehaviour
	{
		[Serialize] public bool hasAskedUserAboutAbyssalite;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (!hasAskedUserAboutAbyssalite)
				AskPlayerAboutAbyssalite();
		}

		private void AskPlayerAboutAbyssalite()
		{
		}

	}
}
