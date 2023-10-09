using Moonlet.Utils.YamlDotNextExtension;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class TraitTemplate : TemplateBase, ITemplate
	{
		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }

		[YamlIgnore] public string Id { get; set; }

		[YamlIgnore] public string Name { get; set; } // base entry has name

	}
}
