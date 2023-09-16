using Moonlet.Templates;
using Moonlet.Utils;
using System.IO;

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

	}

	/// <summary>
	/// Holds content loaded by a single mod
	/// </summary>
	/// <typeparam name="TemplateType">The template describing the YAML file</typeparam>
	public abstract class TemplateLoaderBase<TemplateType>(TemplateType template) : TemplateLoaderBase where TemplateType : ITemplate
	{
		public TemplateType template = template;
	}
}
