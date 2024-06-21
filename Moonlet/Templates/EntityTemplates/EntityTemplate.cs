using System;
using System.Collections.Generic;

namespace Moonlet.Templates.EntityTemplates
{
	public abstract class EntityTemplate : ITemplate
	{
		public string Id { get; set; }

		public bool AddToSandboxMenu { get; set; } = true;

		public string Name { get; set; }

		public string Description { get; set; }

		public string EffectDescription { get; set; }

		public float Width { get; set; }

		public float Height { get; set; }

		public string Element { get; set; }

		public float Mass { get; set; }

		public FloatNumber DefaultTemperature { get; set; }

		public DecorEntry Decor { get; set; }

		public AnimationEntry Animation { get; set; }

		public string[] Tags { get; set; }

		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }

		[Serializable]
		public class AnimationEntry
		{
			public string File { get; set; }

			public string DefaultAnimation { get; set; }

			KAnim.PlayMode PlayMode { get; set; }

			public float Scale { get; set; }

			public string Tint { get; set; }
		}


		[Serializable]
		public class RangeEntry(float low, float high)
		{
			public float Low { get; set; } = low;

			public float High { get; set; } = high;
		}


		[Serializable]
		public class DecorEntry
		{
			public int Range { get; set; }

			public int Value { get; set; }

			public EffectorValues Get() => new EffectorValues(Value, Range);
		}

	}
}