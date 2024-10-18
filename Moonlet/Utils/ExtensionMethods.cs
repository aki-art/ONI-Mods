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
		public static IEnumerable<SoundInfoPair> GetAllEvents(this AudioSheet.SoundInfo soundInfo)
		{
			int index = 0;
			yield return GetEvent(soundInfo, index++);
		}

		public static SoundInfoPair GetEvent(this AudioSheet.SoundInfo soundInfo, int index)
		{
			return index switch
			{
				0 => soundInfo.Name0.IsNullOrWhiteSpace() ? null : new(soundInfo.Name0, soundInfo.Frame0),
				1 => soundInfo.Name1.IsNullOrWhiteSpace() ? null : new(soundInfo.Name1, soundInfo.Frame1),
				2 => soundInfo.Name2.IsNullOrWhiteSpace() ? null : new(soundInfo.Name2, soundInfo.Frame2),
				3 => soundInfo.Name3.IsNullOrWhiteSpace() ? null : new(soundInfo.Name3, soundInfo.Frame3),
				4 => soundInfo.Name4.IsNullOrWhiteSpace() ? null : new(soundInfo.Name4, soundInfo.Frame4),
				5 => soundInfo.Name5.IsNullOrWhiteSpace() ? null : new(soundInfo.Name5, soundInfo.Frame5),
				6 => soundInfo.Name6.IsNullOrWhiteSpace() ? null : new(soundInfo.Name6, soundInfo.Frame6),
				7 => soundInfo.Name7.IsNullOrWhiteSpace() ? null : new(soundInfo.Name7, soundInfo.Frame7),
				8 => soundInfo.Name8.IsNullOrWhiteSpace() ? null : new(soundInfo.Name8, soundInfo.Frame8),
				9 => soundInfo.Name9.IsNullOrWhiteSpace() ? null : new(soundInfo.Name9, soundInfo.Frame9),
				10 => soundInfo.Name10.IsNullOrWhiteSpace() ? null : new(soundInfo.Name10, soundInfo.Frame10),
				11 => soundInfo.Name11.IsNullOrWhiteSpace() ? null : new(soundInfo.Name11, soundInfo.Frame11),
				_ => null,
			};
		}

		public static void SetEventData(this AudioSheet.SoundInfo soundInfo, int index, string name, int frame)
		{
			switch (index)
			{
				case 0:
					soundInfo.Name0 = name;
					soundInfo.Frame0 = frame;
					break;
				case 1:
					soundInfo.Name1 = name;
					soundInfo.Frame1 = frame;
					break;
				case 2:
					soundInfo.Name2 = name;
					soundInfo.Frame2 = frame;
					break;
				case 3:
					soundInfo.Name3 = name;
					soundInfo.Frame3 = frame;
					break;
				case 4:
					soundInfo.Name4 = name;
					soundInfo.Frame4 = frame;
					break;
				case 5:
					soundInfo.Name5 = name;
					soundInfo.Frame5 = frame;
					break;
				case 6:
					soundInfo.Name6 = name;
					soundInfo.Frame6 = frame;
					break;
				case 7:
					soundInfo.Name7 = name;
					soundInfo.Frame7 = frame;
					break;
				case 8:
					soundInfo.Name8 = name;
					soundInfo.Frame8 = frame;
					break;
				case 9:
					soundInfo.Name9 = name;
					soundInfo.Frame9 = frame;
					break;
				case 10:
					soundInfo.Name10 = name;
					soundInfo.Frame10 = frame;
					break;
				case 11:
					soundInfo.Name11 = name;
					soundInfo.Frame11 = frame;
					break;
			}
		}

		public class SoundInfoPair(string name, int frame)
		{
			public string name = name;
			public int frame = frame;
		}

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
