using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecorPackA.Scripts
{
	// save per world data about the mod not specific to a single building
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DecorPackA_Mod : KMonoBehaviour//, ISim33ms, ISim200ms
	{
		public static DecorPackA_Mod Instance;

		[Serialize][SerializeField] public bool showHSV;
		[Serialize][SerializeField] public bool showSwatches;

		//private List<float> frameRateCache = new();

		public const int MAX_FRAMES = 60;
		public float fps;

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

/*		// not at all accurate, it's a vanity counter
		public void Sim33ms(float dt)
		{
			frameRateCache.Add(Time.unscaledDeltaTime);

			if (frameRateCache.Count > MAX_FRAMES)
				frameRateCache.RemoveAt(0);
		}

		public void Sim200ms(float dt)
		{
			fps = frameRateCache.Count() / frameRateCache.Sum();
		}*/
	}
}
