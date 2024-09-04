extern alias YamlDotNetButNew;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates
{
	public class AchievementTweaksTemplate
	{
		public ModifyAchievements Modify { get; set; }

		public class ModifyAchievements
		{
			[YamlMember(Alias = "GMOOK")]
			public GMO_OK GMOOK { get; set; }
		}

		public class AchievementModifierBase
		{
			public bool Disable { get; set; }
		}

		public class GMO_OK : AchievementModifierBase
		{
			public List<string> AddSeeds { get; set; }
		}
	}
}