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
		private static readonly Dictionary<string, Type> componentMappings = new()
		{
			{ "Edible", typeof(EdibleComponent) },
			{ "Sublimates", typeof(SublimatesComponent) },
		};



		public override IDeserializer CreateDeserializer()
		{
			Log.Debug("CREATING DESERIALIZER");
			return new DeserializerBuilder()
				.IncludeNonPublicProperties()
				.IgnoreUnmatchedProperties()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.WithTypeDiscriminatingNodeDeserializer((o) =>
				{
					IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
					{
						{ "Edible", typeof(EdibleComponent) },
						{ "Sublimates", typeof(SublimatesComponent) },
					};

					Log.Debug("added mappings: " + valueMappings.Count);
					o.AddKeyValueTypeDiscriminator<BaseComponent>("type", valueMappings); // "type" must match the name of the key exactly as it appears in the Yaml document.
				})
				.Build();
		}
	}
}
