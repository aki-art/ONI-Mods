using Moonlet.Templates.CodexTemplates;
using System.IO;

namespace Moonlet.TemplateLoaders
{
	public class CodexEntryTemplateLoader(CodexEntryTemplate template, string sourceMod) : TemplateLoaderBase<CodexEntryTemplate>(template, sourceMod)
	{
		private CodexEntry cachedEntry;
		private string parent;

		public override void Initialize()
		{
			base.Initialize();

			if (template.Id.IsNullOrWhiteSpace())
			{
				if (template.Id.IsNullOrWhiteSpace())
				{
					var id = Path.GetFileNameWithoutExtension(relativePath).ToUpperInvariant();
					template.Id = id;
					this.id = id;
				}

				parent = Path.GetFileName(Path.GetDirectoryName(relativePath)).ToUpperInvariant();
			}
		}

		public CodexEntry Get()
		{
			var entry = template.Convert(Warn);
			entry.category = parent; // TODO

			return entry;
		}

		public void AddToCache()
		{
			var entry = Get();
			if (entry != null)
			{
				if (CodexCache.entries.ContainsKey(template.Id))
				{
					CodexCache.MergeEntry(template.Id, entry);
				}
				else
					CodexCache.AddEntry(template.Id, entry);
			}
			else
				Warn("Could not load Codex entry.");
		}

		public override void RegisterTranslations()
		{
			var path = relativePath.Replace("/", ".").ToUpperInvariant();

			var stringsRoot = $"STRINGS.CODEX.{path}.{template.Id.ToUpperInvariant()}";

			if (template.ContentContainers == null)
				return;

			foreach (var container in template.ContentContainers)
			{
				for (var i = 0; i < container.Content.Count; i++)
				{
					var content = container.Content[i];
					switch (content)
					{
						case TextEntry text:
							var key = $"{stringsRoot}.CONTAINER{i}";
							AddString(key, text.Text);
							text.SringKey = key;
							break;

							/*						case CodexTextWithTooltip textWithTooltip:
														var key1 = $"{stringsRoot}.CONTAINER{i}";
														var key2 = $"{stringsRoot}.TOOLTIP{i}";
														AddString(key1, textWithTooltip.text);
														AddString(key2, textWithTooltip.tooltip);
														break;
													case CodexCollapsibleHeader collapsibleHeader:
														AddString($"{stringsRoot}.HEADER{i}", collapsibleHeader.label);
														break;*/
					}
				}
			}
		}

		public void LoadContent()
		{

		}
	}
}
