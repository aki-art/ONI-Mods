using Moonlet.Scripts.ComponentTypes;
using Moonlet.TemplateLoaders.EntityLoaders;
using Moonlet.Templates.EntityTemplates;
using System;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class EntitiesLoader<EntityType>(string path) : TemplatesLoader<EntityLoaderBase<EntityType>>(path) where EntityType : EntityTemplate
	{
		private static Dictionary<string, Type> componentMappings = new()
		{
			{ "edible", typeof(EdibleComponent) }
		};

		protected override Dictionary<string, Type> GetMappings()
		{
			return componentMappings;
		}
	}
}
