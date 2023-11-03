using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class TemplateCollection2<TemplateType> : Dictionary<string, List<TemplateType>>
	{
		private static HashSet<string> ignoredKeys = new()
		{
			"variables",
			"remove"
		};

		public List<TemplateType> Templates()
		{
			var result = new List<TemplateType>();
			foreach (var kvp in this)
			{
				var id = kvp.Key.ToLowerInvariant();

				if (kvp.Value == null || ignoredKeys.Contains(id))
					continue;

				result.AddRange(kvp.Value);
			}

			return result;
		}
	}
}
