using Moonlet.Templates.SubTemplates;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moonlet.Utils
{
	public class ShadowTypeUtil
	{
		public static Dictionary<string, OriginalType> CopyDictionary<OriginalType, SubTemplateType>(Dictionary<string, SubTemplateType> template, Action<string> log) where SubTemplateType : ShadowTypeBase<OriginalType>
		{
			if (template == null)
				return null;

			var result = new Dictionary<string, OriginalType>();

			foreach (var item in template)
				result[item.Key] = item.Value.Convert(log);

			return result;
		}

		public static void SoftCopyProperties(object from, object to)
		{
			Log.Debug("soft copying");
			if (to.GetType() != from.GetType())
				return;

			Log.Debug("type ok");

			var templateType = from.GetType();

			var targetProperties = from.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);


			foreach (var originalProperty in targetProperties)
			{
				if (originalProperty.Name == "Priority") // we live for the jank
					continue;

				var templateProperty = templateType.GetProperty(originalProperty.Name);

				if (templateProperty != null && templateProperty.PropertyType == originalProperty.PropertyType)
				{
					var templateValue = templateProperty.GetValue(from);

					if (templateValue != null)
					{
						originalProperty.SetValue(to, templateValue);
					}
				}
			}
		}

		public static List<OriginalType> CopyList<OriginalType, SubTemplateType>(List<SubTemplateType> template, Action<string> log) where SubTemplateType : ShadowTypeBase<OriginalType>
		{
			if (template == null)
				return null;

			var result = new List<OriginalType>();

			foreach (var item in template)
				result.Add(item.Convert(log));

			return result;
		}
	}
}
