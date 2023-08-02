using Moonlet.Entities.Commands;
using Moonlet.Entities.ComponentTypes;
using System;
using System.Collections.Generic;
using TUNING;

namespace Moonlet.Entities
{
	public class EntityData
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string EffectDescription { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Element { get; set; }

		public float Mass { get; set; }

		public float? DefaultTemperature { get; set; }

		public float? DefaultTemperatureCelsius { get; set; }

		public DecorEntry Decor { get; set; }

		public AnimationEntry Animation { get; set; }

		public List<BaseComponent> Components { get; set; }

		public List<BaseCommand> OnSpawn { get; set; }

		public List<BaseCommand> OnDestroy { get; set; }

		public string[] Tags { get; set; }

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
		public class DecorEntry
		{
			public int Range { get; set; }

			public int Value { get; set; }
		}

		public EffectorValues GetDecor() => Decor != null ? new EffectorValues(Decor.Value, Decor.Range) : DECOR.NONE;
	}
}
