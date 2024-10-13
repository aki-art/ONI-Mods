extern alias YamlDotNetButNew;
using Moonlet.Scripts.ComponentTypes;
using Moonlet.TemplateLoaders.EntityLoaders;
using Moonlet.Templates.EntityTemplates;
using System;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;
using YamlDotNetButNew.YamlDotNet.Serialization.NamingConventions;

namespace Moonlet.Loaders
{
	public class EntitiesLoader<EntityLoaderType, EntityTemplateType>(string path) : TemplatesLoader<EntityLoaderType>(path)
		where EntityLoaderType : EntityLoaderBase<EntityTemplateType>
		where EntityTemplateType : EntityTemplate
	{
		public override IDeserializer CreateDeserializer()
		{
			return new DeserializerBuilder()
				.IncludeNonPublicProperties()
				.IgnoreUnmatchedProperties()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.WithTypeDiscriminatingNodeDeserializer((o) =>
				{
					IDictionary<string, Type> valueMappings = Mod.componentTypes;

					o.AddKeyValueTypeDiscriminator<BaseComponent>("type", valueMappings); // "type" must match the name of the key exactly as it appears in the Yaml document.
				})
				.Build();
		}
	}
}
