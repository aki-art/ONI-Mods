using Moonlet.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;

namespace Moonlet.Utils
{
	public static class ExtensionMethods
	{
		public static string[] GetDlcIds(this ITemplate template)
		{
			return template.Conditions == null ? DlcManager.AVAILABLE_ALL_VERSIONS : template.Conditions.DlcIds;
		}

		public static string[] GetModIds(this ITemplate template)
		{
			return template.Conditions == null ? [] : template.Conditions.Mods;
		}

		public static string LinkAppropiateFormat(this string link, bool trimStart = true)
		{
			if (trimStart)
				link = link.TrimStart('/', ' ');

			return global::STRINGS.UI.StripLinkFormatting(link)
				.Replace(" ", "_")
				.Replace("/", "_")
				.ToUpperInvariant();
		}

		public static NumberType CalculateOrDefault<NumberType>(this NumberBase<NumberType> expression, NumberType defaultValue = default) where NumberType : struct
		{
			return expression == null ? defaultValue : expression.Calculate();
		}

		// https://stackoverflow.com/a/58242870
		public static Dictionary<string, PropertyInfo> GetDeserializableProperties(this Type type) => type.GetProperties()
	.Select(p => new KeyValuePair<string, PropertyInfo>(p.GetCustomAttributes<YamlMemberAttribute>(true).Select(yma => yma.Alias).FirstOrDefault(), p))
	.Where(pa => !pa.Key.IsNullOrWhiteSpace()).ToDictionary(pa => pa.Key, pa => pa.Value);


		// https://stackoverflow.com/a/58242870
		// Only allows deserialization of properties that are primitives or type Dictionary<object, object>. Does not support properties that are custom classes.
		public static object DeserializeDictionary(this PropertyInfo info, object value)
		{
			if (!(value is Dictionary<object, object>)) return TypeConverter.ChangeType(value, info.PropertyType);

			var type = info.PropertyType;
			var properties = type.GetDeserializableProperties();
			var property = Activator.CreateInstance(type);
			var matchedProperties = ((Dictionary<object, object>)value).Where(e => properties.ContainsKey(e.Key.ToString()));
			foreach (var (propKey, propValue) in matchedProperties)
			{
				var innerInfo = properties[propKey.ToString()];
				innerInfo.SetValue(property, innerInfo.DeserializeDictionary(propValue));
			}
			return property;
		}
	}
}
