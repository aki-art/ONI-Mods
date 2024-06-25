extern alias YamlDotNetButNew;
using System;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates
{
	// TODO: enum extensions
	public class EnumEntry<EnumType> : IYamlConvertible where EnumType : struct, Enum
	{
		public EnumType value;

		public EnumEntry()
		{
		}

		public EnumEntry(string str)
		{
			TryParse(str, out value);
		}

		public EnumEntry(EnumType value)
		{
			this.value = value;
		}

		public override string ToString() => value.ToString();

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			if (parser.TryConsume<Scalar>(out var scalarValue))
			{
				TryParse(scalarValue.Value, out value);
			}
			else
			{
				var values = (EnumEntry<EnumType>)nestedObjectDeserializer(typeof(EnumEntry<EnumType>));
				value = values.value;
			}
		}

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			nestedObjectSerializer(new EnumEntry<EnumType>(value));
		}

		private bool TryParse(string input, out EnumType result)
		{
			result = default;

			if (!input.IsNullOrWhiteSpace())
			{
				try
				{
					result = (EnumType)Enum.Parse(typeof(EnumType), input); // using Parse instead of TryParse because mods tend to only patch Parse
					return true;
				}
				catch
				{
					// TODO: lookup extensions 
				}

			}

			return true;
		}
	}
}
