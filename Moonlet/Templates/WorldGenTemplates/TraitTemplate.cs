using ProcGen;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class TraitTemplate : WorldTrait, ITemplate
	{
		[YamlIgnore]
		public string Id { get; set; }

		[YamlIgnore]
		public string Name
		{
			get => name;
			set => name = value;
		}

		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
