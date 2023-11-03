using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Utils
{
	public class ShadowTypeUtil
	{
		public static Dictionary<string, OriginalType> CopyDictionary<OriginalType, SubTemplateType>(Dictionary<string, SubTemplateType> template) where SubTemplateType : ShadowTypeBase<OriginalType>
		{
			if (template == null)
				return null;

			var result = new Dictionary<string, OriginalType>();

			foreach (var item in template)
				result[item.Key] = item.Value.Convert();

			return result;
		}
		public static List<OriginalType> CopyList<OriginalType, SubTemplateType>(List<SubTemplateType> template) where SubTemplateType : ShadowTypeBase<OriginalType>
		{
			if (template == null)
				return null;

			var result = new List<OriginalType>();

			foreach (var item in template)
				result.Add(item.Convert());

			return result;
		}
	}
}
