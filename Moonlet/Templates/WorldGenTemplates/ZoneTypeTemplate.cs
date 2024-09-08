﻿using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ZoneTypeTemplate : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }

		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public ColorEntry Color { get; set; }

		public string Border { get; set; }

		public TextureEntry Background { get; set; }

		public int BackgroundIndex { get; set; }
	}
}
