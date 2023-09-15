using Moonlet.Templates;

namespace Moonlet
{
	public abstract class TemplateLoaderBase
	{
		public string id;

		public string sourceMod;

		public bool stomped;

		public int priority;
	}

	/// <summary>
	/// Holds content loaded by a single mod
	/// </summary>
	/// <typeparam name="TemplateType">The template describing the YAML file</typeparam>
	public abstract class TemplateLoaderBase<TemplateType> : TemplateLoaderBase where TemplateType : ITemplate
	{
		public TemplateType template;
	}
}
