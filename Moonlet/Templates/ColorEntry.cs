extern alias YamlDotNetButNew;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates
{
	public class ColorEntry : IYamlConvertible
	{
		public Color value;
		public bool hasValue;

		public ColorEntry()
		{
		}

		public ColorEntry(string str)
		{
			hasValue = TryParse(str, out value);
		}

		public ColorEntry(Color color)
		{
			value = color;
			hasValue = true;
		}

		public override string ToString()
		{
			return value == null ? "null" : value.ToString();
		}

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			var scalar = parser.Allow<Scalar>();
			if (scalar != null)
			{
				TryParse(scalar.Value, out value);
			}
			else
			{
				var values = (ColorEntry)nestedObjectDeserializer(typeof(ColorEntry));
				value = values.value;
			}
		}

		public static implicit operator Color(ColorEntry entry) => entry.value;
		public static implicit operator ColorEntry(Color color) => new(color);

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			nestedObjectSerializer(new ColorEntry(value));
		}

		private bool TryParse(string str, out Color result)
		{
			result = Color.magenta;

			if (str.IsNullOrWhiteSpace())
				return false;

			str = str.Replace(" ", "");

			if (str.Contains(','))
			{
				var components = str.Split(',');
				if (components.Length >= 3 && components.Length <= 4)
				{
					if (float.TryParse(components[0], out var r)
						&& float.TryParse(components[1], out var g)
						&& float.TryParse(components[2], out var b))
					{
						var color = new Color(r, g, b);

						if (components.Length == 4 && float.TryParse(components[3], out var a))
							color.a = a;

						result = color;
						hasValue = true;
						return true;
					}
				}
			}

			if (int.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out var _))
			{
				result = Util.ColorFromHex(str);
				hasValue = true;
				return true;
			}

			Log.Warn($"{str} is not a valid color format. Expected: FFFFFF or 1,1,1,1");

			hasValue = false;
			return false;
		}
	}
}
