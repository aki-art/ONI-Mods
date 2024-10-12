extern alias YamlDotNetButNew;

using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class BorderTemplate : BaseTemplate
	{
		public Dictionary<string, List<WeightedSimHash>> Add { get; set; }
		public List<string> Remove { get; set; }
	}
}
