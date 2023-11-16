using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class TemperatureTemplate : ITemplate
	{
		public Dictionary<string, MinMaxC> Add { get; set; }
		public List<string> Remove { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
