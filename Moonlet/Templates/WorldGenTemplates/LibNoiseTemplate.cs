using Moonlet.Templates.SubTemplates.Noise;
using ProcGen.Noise;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class LibNoiseTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerClusterTag { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }

		public SampleSettingsC Settings { get; set; }
		public List<NodeLink> Links { get; set; }
		public Dictionary<string, PrimitiveC> Primitives { get; set; }
		public Dictionary<string, FilterC> Filters { get; set; }
		public Dictionary<string, TransformerC> Transformers { get; set; }
		public Dictionary<string, Selector> Selectors { get; set; }
		public Dictionary<string, ModifierC> Modifiers { get; set; }
		public Dictionary<string, CombinerC> Combiners { get; set; }
		public Dictionary<string, FloatList> Floats { get; set; }
		public Dictionary<string, ControlPointList> Controlpoints { get; set; }

		public LibNoiseTemplate()
		{
			Settings = new();
			Primitives = [];
			Modifiers = [];
			Combiners = [];
		}
	}
}
