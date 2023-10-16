using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class TemplateCollection2<TemplateType> : Dictionary<string, List<TemplateType>>
	{
		public List<TemplateType> Templates()
		{
			var result = new List<TemplateType>();
			foreach (var kvp in this)
			{
				if (kvp.Value == null || kvp.Key.ToLowerInvariant() == "variables")
					continue;

				result.AddRange(kvp.Value);
			}

			return result;
		}
	}
}
