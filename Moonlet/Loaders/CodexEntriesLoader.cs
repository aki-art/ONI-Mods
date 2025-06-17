extern alias YamlDotNetButNew;

using Moonlet.TemplateLoaders;
using Moonlet.Templates.CodexTemplates;
using System;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;
using YamlDotNetButNew.YamlDotNet.Serialization.NamingConventions;
using Path = System.IO.Path;

namespace Moonlet.Loaders
{
	public class CodexEntriesLoader(string path) : TemplatesLoader<CodexEntryTemplateLoader>(path)
	{
		public static readonly string CREATURES = Path.Combine("codex", "creatures");
		private static Dictionary<string, Type> legacyMappings;

		public override IDeserializer CreateDeserializer()
		{
			InitializeMappings();
			return new DeserializerBuilder()
				.IncludeNonPublicProperties()
				.IgnoreUnmatchedProperties()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.WithTypeDiscriminatingNodeDeserializer((o) =>
				{
					o.AddKeyValueTypeDiscriminator<BaseWidgetTemplate>("type", legacyMappings);
				})
				.Build();
		}
		private void InitializeMappings()
		{
			if (legacyMappings != null)
				return;

			legacyMappings = [];
			var mappings = new Dictionary<string, Type>
			{
				{"Text", typeof(TextEntry) },
				{"DividerLine", typeof(DividerLineEntry)},
				{"Image", typeof(ImageEntry)},
				/*{"Spacer", typeof(CodexSpacer)},
				{"LabelWithIcon", typeof(CodexLabelWithIcon)},
				{"LabelWithLargeIcon", typeof(CodexLabelWithLargeIcon)},
				{"ContentLockedIndicator", typeof(CodexContentLockedIndicator)},
				{"LargeSpacer", typeof(CodexLargeSpacer)},
				{"Video", typeof(CodexVideo)},*/
			};

			foreach (var mapping in mappings)
			{
				legacyMappings[mapping.Key] = mapping.Value;
				legacyMappings["Codex" + mapping.Key] = mapping.Value;
			}

		}
	}
}
