extern alias YamlDotNetButNew;

using Moonlet.DocGen;
using Moonlet.Utils.MxParser;
using System;
using System.Linq;
using UnityEngine;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates
{
	public class NumberBase<NumberType> : IYamlConvertible where NumberType : struct
	{
		protected MExpression expression;
		public string expressionString;
		public bool initialized;

		public NumberBase(string expression)
		{
			expressionString = expression;
		}

		public NumberBase()
		{
		}

		public virtual string PreProcess(string expressionString) => expressionString;

		public void SetExpression(string expressionString)
		{
			this.expressionString = PreProcess(expressionString);
			expression = new(this.expressionString);
			initialized = true;
		}

		public NumberType Calculate()
		{
			if (!initialized)
				SetExpression(expressionString);

			return Calculate_internal();
		}

		protected virtual NumberType Calculate_internal() => default;

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			if (parser.TryConsume<Scalar>(out var ev))
			{
				expressionString = ev.Value;
			}
			else
			{
				var values = (NumberBase<NumberType>)nestedObjectDeserializer(typeof(NumberBase<NumberType>));
				expressionString = values.expressionString;
			}
		}

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			nestedObjectSerializer(new NumberBase<NumberType>(expressionString));
		}

	}

	public class IntNumber : NumberBase<int>, IDocumentation
	{
		protected override int Calculate_internal() => (int)expression.calculate();

		public static implicit operator int(IntNumber number) => number.Calculate();
		public static implicit operator IntNumber(int number) => new()
		{
			expressionString = number.ToString()
		};
		public void ModifyDocs(DocPage page)
		{
			page.description = "Whole numbers. Accepts mathematical functions too. Fractions will be rounded down."; //TODO: math docs
		}
	}

	public class FloatNumber : NumberBase<float>, IDocumentation
	{
		protected override float Calculate_internal() => (float)expression.calculate();

		public static implicit operator float(FloatNumber number) => number.Calculate();
		public static implicit operator FloatNumber(float number) => new()
		{
			expressionString = number.ToString()
		};

		public void ModifyDocs(DocPage page)
		{
			page.description = "A real number, whole or fractions. Accepts mathematical functions too."; //TODO: math docs
		}
	}

	/// <summary>
	/// Accepts all formatting FloatNumber does, but also takes a K, C or F suffix at the very end, allowing you to set temperatures in your preferred metric. The default is Kelvin.
	/// </summary>
	public class TemperatureNumber : NumberBase<float>, IDocumentation
	{
		private GameUtil.TemperatureUnit _unit;
		private GameUtil.TemperatureUnit Unit
		{
			get => _unit;
			set
			{
				_unit = value;
				hasUnit = true;
			}
		}

		private bool hasUnit;

		public override string PreProcess(string expressionString)
		{
			if (!expressionString.IsNullOrWhiteSpace())
			{
				expressionString = expressionString.Replace(" ", "");
				var lastChar = expressionString.Last();

				switch (lastChar)
				{
					case 'K':
						Unit = GameUtil.TemperatureUnit.Kelvin;
						break;
					case 'C':
						Unit = GameUtil.TemperatureUnit.Celsius;
						break;
					case 'F':
						Unit = GameUtil.TemperatureUnit.Fahrenheit;
						break;
				}

				if (hasUnit)
					expressionString = expressionString.Remove(expressionString.Length - 1, 1);
			}

			return expressionString;

		}

		public static implicit operator float(TemperatureNumber number) => number.Calculate();

		public static implicit operator TemperatureNumber(float number) => new()
		{
			expressionString = number.ToString()
		};

		protected override float Calculate_internal()
		{
			if (expressionString != null)
				expression.setExpressionString(expressionString);
			var result = (float)expression.calculate();

			if (hasUnit)
				result = GameUtil.GetTemperatureConvertedToKelvin(result, Unit);

			if (result != float.NaN)
				result = Mathf.Clamp(result, 0, 9975);

			return result;
		}

		public void ModifyDocs(DocPage page)
		{
			page.description = "A numeric entry that accepts fractions. Put C, K or F at the very end to convert between temperature units. For example, 343 K or 45 C. The default if unmarked is Kelvin.";
		}
	}
}
