using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class WorldMixingTemplate : BaseTemplate
	{
		public string Description { get; private set; }

		public string Icon { get; private set; }

		public List<string> ForbiddenClusterTags { get; private set; }

		public string World { get; private set; }

		public WorldMixingTemplate()
		{
			ForbiddenClusterTags = [];
		}
	}
}
