using System.Collections;
using System.Collections.Generic;

namespace Moonlet
{
	public class TemplateLoaderCollection<TemplateLoaderType> : IEnumerable<TemplateLoaderType> where TemplateLoaderType : TemplateLoaderBase
	{
		private Dictionary<string, TemplateLoaderType> templates;

		public TemplateLoaderCollection()
		{
			templates = new ();
		}

		public virtual bool Add(TemplateLoaderType template)
		{
			if (templates.TryGetValue(template.id, out var existingTemplate))
				if (existingTemplate.priority >= template.priority)
				{

					if (existingTemplate.priority == template.priority)
					{
						Log.Warning($"Conflicting templates with identical priorities: {template.id}, added by {template.sourceMod} and {existingTemplate.sourceMod}. Keeping {existingTemplate.sourceMod}.");
					}
						return false;
				}
				else
					RemovedTemplate(existingTemplate);

			templates[template.id] = template;
			return true;
		}

		public IEnumerator<TemplateLoaderType> GetEnumerator() => templates.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		private void RemovedTemplate(TemplateLoaderType existingTemplate)
		{

		}
	}
}
