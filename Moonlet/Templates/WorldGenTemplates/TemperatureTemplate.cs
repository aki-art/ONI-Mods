using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class TemperatureTemplate : ComposableDictionary<string, MinMaxC>, ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
