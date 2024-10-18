extern alias YamlDotNetButNew;

using Moonlet.Scripts.Commands;
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
					IDictionary<string, Type> componentMappings = Mod.componentTypes;
					o.AddKeyValueTypeDiscriminator<BaseComponent>("type", componentMappings); // "type" must match the name of the key exactly as it appears in the Yaml document.

					IDictionary<string, Type> commandMappings = Mod.commandTypes;
					o.AddKeyValueTypeDiscriminator<BaseCommand>("command", commandMappings);
				})
				//.WithNodeDeserializer(new ForceEmptyContainer())
				.Build();
		}
	}
}
