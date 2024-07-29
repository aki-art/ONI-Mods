using Moonlet.Templates;
using System.Collections.Generic;

namespace Moonlet.Scripts
{
	public class Moonlet_Mod : KMonoBehaviour
	{
		public static Moonlet_Mod Instance;

		public static Dictionary<SimHashes, ElementTemplate.EffectsEntry> stepOnEffects;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}
	}
}
