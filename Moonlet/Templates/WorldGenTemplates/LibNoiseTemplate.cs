﻿using Moonlet.Templates.SubTemplates.Noise;
using ProcGen.Noise;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class LibNoiseTemplate : BaseTemplate
	{
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
