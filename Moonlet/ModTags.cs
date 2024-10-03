using System.Collections.Generic;

namespace Moonlet
{
	public class ModTags
	{
		public static HashSet<Tag> hiddenTags = [];

		public static readonly Tag
			SafeAtmosphereForPlants = TagManager.Create("Moonlet_SafeAtmosphere");

		public static class EffectTags
		{
			public static readonly Tag
				Soaked = TagManager.Create("Moonlet_Soaked"),
				WetFeet = TagManager.Create("Moonlet_WetFeet");
		}
	}
}
