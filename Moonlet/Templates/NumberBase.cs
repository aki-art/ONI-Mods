using Moonlet.Utils.MxParser;
using System;
using System.Linq;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

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
			var scalar = parser.Allow<Scalar>();
			if (scalar != null)
			{
				expressionString = scalar.Value;
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

	public class IntNumber : NumberBase<int>
	{
		protected override int Calculate_internal() => (int)expression.calculate();
	}

	public class FloatNumber : NumberBase<float>
	{
		protected override float Calculate_internal() => (float)expression.calculate();
	}

	public class TemperatureNumber : NumberBase<float>
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
				/*				expressionString = expressionString.Replace(" ", "");
								expressionString = Regex.Replace(expressionString, "[0-9.]+K", ReplaceKelvin);
								expressionString = Regex.Replace(expressionString, "[0-9.]+C", ReplaceCelsius);
								expressionString = Regex.Replace(expressionString, "[0-9.]+F", ReplaceFahrenheit);*/

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
		/*
				private string ReplaceKelvin(Match match)
				{
					Unit = GameUtil.TemperatureUnit.Kelvin;
					return match.Value.Replace("K", "");
				}

				private string ReplaceFahrenheit(Match match)
				{
					if (hasUnit && Unit != GameUtil.TemperatureUnit.Fahrenheit)
					{
						Log.Warn($"Error in expression `{expressionString}`, mixed temperature units not supported!");
						return match.Value;
					}

					Unit = GameUtil.TemperatureUnit.Fahrenheit;
					return match.Value.Replace("F", "");
				}

				private string ReplaceCelsius(Match match)
				{
					if (hasUnit && Unit != GameUtil.TemperatureUnit.Celsius)
					{
						Log.Warn($"Error in expression `{expressionString}`, mixed temperature units not supported!");
						return match.Value;
					}

					Unit = GameUtil.TemperatureUnit.Celsius;
					return match.Value.Replace("C", "");
				}
		*/
		protected override float Calculate_internal()
		{
			var result = (float)expression.calculate();

			if (hasUnit)
				result = GameUtil.GetTemperatureConvertedToKelvin(result, Unit);

			if (result != float.NaN)
				result = Mathf.Clamp(result, 0, 9975);

			return result;
		}
	}
}
