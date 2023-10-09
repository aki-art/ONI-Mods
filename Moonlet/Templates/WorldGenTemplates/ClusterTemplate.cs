using Moonlet.Utils.YamlDotNextExtension;
using ProcGen;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ClusterTemplate : TemplateBase, ITemplate
	{
		public string Priority { get; set; }

		[YamlIgnore] public Dictionary<string, string> PriorityPerCluster { get; set; }

		public string Id { get; set; }

		[YamlIgnore] public string Name { get; set; } // base entry has name

	}
}
