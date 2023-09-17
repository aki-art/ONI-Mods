using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public abstract class TemplateLoaderBase
	{
		public string id;
		public string sourceMod;
		public bool isActive;
		public int priority;

		public abstract void RegisterTranslations();

		public virtual string GetTranslationKey(string partialKey) => partialKey;

		public virtual int GetPriority(string clusterId) => priority;
	}

	/// <summary>
	/// Holds content loaded by a single mod
	/// </summary>
	/// <typeparam name="TemplateType">The template describing the YAML file</typeparam>
	public abstract class TemplateLoaderBase<TemplateType> : TemplateLoaderBase where TemplateType : ITemplate
	{
		public TemplateType template;

		public TemplateLoaderBase(TemplateType template)
		{
			this.template = template;
			isActive = true;
			id = template.Id;
		}

		public override int GetPriority(string clusterId)
		{
			if (clusterId != null
				&& template.PriorityPerCluster != null
				&& template.PriorityPerCluster.TryGetValue(clusterId, out var priority))
				return priority;

			return template.Priority;
		}
	}
}
