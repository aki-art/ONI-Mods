using Moonlet.Scripts.ComponentTypes;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.EntityTemplates
{
	public class EntityTemplate : BaseTemplate
	{
		public bool Pickupable { get; set; }

		public ObjectLayer[] Layers { get; set; }

		public bool AddToSandboxMenu { get; set; } = true;

		public string Description { get; set; }

		public string EffectDescription { get; set; }

		public float Width { get; set; }

		public float Height { get; set; }

		public string Element { get; set; }

		public float Mass { get; set; }

		public TemperatureNumber DefaultTemperature { get; set; }

		public DecorEntry Decor { get; set; }

		public AnimationEntry Animation { get; set; }

		public string[] Tags { get; set; }

		public string[] DLC { get; set; }

		public List<BaseComponent> Components { get; set; }

		public EntityTemplate()
		{
			Mass = 30.0f;
			Element = SimHashes.Creature.ToString();
			DefaultTemperature = GameUtil.GetTemperatureConvertedToKelvin(20, GameUtil.TemperatureUnit.Celsius);
			AddToSandboxMenu = true;
			Width = 1;
			Height = 1;
		}

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
		public class ComponentyEntry
		{
			public BaseComponent Value { get; set; }
		}


		[Serializable]
		public class Parent // ComponentyEntry
		{
			public string Property1 { get; set; }
			public string Property2 { get; set; }
			public TypeObject Value { get; set; }
		}

		[Serializable]
		public abstract class TypeObject // BaseComponent
		{
			public string ObjectType { get; set; }
		}
		public class Type1 : TypeObject // EdibleComponent
		{
			public string Type1Value { get; set; }
		}
		public class Type2 : TypeObject
		{
			public string Type2Value { get; set; }
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