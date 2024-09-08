using KSerialization;
using Moonlet.Templates;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Scripts
{
	[DefaultExecutionOrder(10)]
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Moonlet_Mod : KMonoBehaviour
	{
		public static Moonlet_Mod Instance;

		public static Dictionary<SimHashes, ElementTemplate.EffectsEntry> stepOnEffects;

		[Serialize]
		public Dictionary<int, string> cachedZoneTypesIndices;

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
