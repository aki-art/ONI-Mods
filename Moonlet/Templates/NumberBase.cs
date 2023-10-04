using Moonlet.Utils.MxParser;
using System;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Moonlet.Templates
{
	public class NumberBase<NumberType> : IYamlConvertible where NumberType : struct
	{
		public MExpression expression;
		public string expressionString;

		public NumberBase(string expression)
		{
			this.expression = new(expression);
			expressionString = expression;
		}

		public NumberBase()
		{
		}

		public NumberType Calculate(NumberType defaultValue = default)
		{
			expression ??= new(expressionString);
			/*
						if (!expression.checkSyntax())
						{
							var errors = expression.getErrorMessage();
							Log.Warn($"Error in expression {expressionString}: ", errors);
						}
			*/
			return Calculate_internal();
		}

		protected virtual NumberType Calculate_internal() => default;

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			var scalar = parser.Allow<Scalar>();
			if (scalar != null)
			{
				expression = new MExpression(scalar.Value);
			}
			else
			{
				var values = (NumberBase<NumberType>)nestedObjectDeserializer(typeof(NumberBase<NumberType>));
				expression = new MExpression(values.expression.getExpressionString());
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

	public class TemperatureEntry : NumberBase<float>
	{
		protected override float Calculate_internal()
		{
			var str = expression.getExpressionString();
			var result = (float)expression.calculate();
			result = Mathf.Clamp(result, 0, 9999);

			return result;
		}
	}
}
