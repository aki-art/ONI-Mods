using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class TemperatureTemplate : BaseTemplate
	{
		public Dictionary<string, MinMaxC> Add { get; set; }
		public List<string> Remove { get; set; }
	}
}
