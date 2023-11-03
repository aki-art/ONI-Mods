﻿using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.SubTemplates.Noise;
using ProcGen.Noise;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class LibNoiseTemplate : ITemplate
	{
		public Vector2FC Test { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerCluster { get; set; }

		public SampleSettingsC Settings { get; set; }
		public List<NodeLink> Links { get; set; }
		public Dictionary<string, PrimitiveC> Primitives { get; set; }
		public Dictionary<string, Filter> Filters { get; set; }
		public Dictionary<string, TransformerC> Transformers { get; set; }
		public Dictionary<string, Selector> Selectors { get; set; }
		public Dictionary<string, ModifierC> Modifiers { get; set; }
		public Dictionary<string, CombinerC> Combiners { get; set; }
		public Dictionary<string, FloatList> Floats { get; set; }
		public Dictionary<string, ControlPointList> Controlpoints { get; set; }

		public LibNoiseTemplate()
		{
			Settings = new();
			Primitives = new();
			Modifiers = new();
			Combiners = new();
			Test = new(8, 9);
		}
	}
}