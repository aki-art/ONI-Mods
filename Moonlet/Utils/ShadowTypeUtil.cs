using Moonlet.Templates.SubTemplates;
using System;
using System.Collections.Generic;

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
