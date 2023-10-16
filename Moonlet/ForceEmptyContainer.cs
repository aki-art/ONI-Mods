extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using System.Linq;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;
using YamlDotNetButNew.YamlDotNet.Serialization.ObjectFactories;

namespace Moonlet
{
	// makes sure that Commands and Components can be empty, so a single !!tag works with a null object defined, and just assumes default values
	// https://github.com/aaubry/YamlDotNet/issues/443#issuecomment-544449498
	public sealed class ForceEmptyContainer : INodeDeserializer
	{
		private readonly IObjectFactory objectFactory = new DefaultObjectFactory();

		public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
		{
			value = null;

			if (IsContainer(expectedType) && parser.TryConsume<NodeEvent>(out var ev))
			{
				if (NodeIsNull(ev))
				{
					parser.SkipThisAndNestedEvents();
					value = objectFactory.Create(expectedType);
					return true;
				}
			}

			return false;
		}

		private bool NodeIsNull(NodeEvent nodeEvent)
		{
			// http://yaml.org/type/null.html

			if (nodeEvent.Tag == "tag:yaml.org,2002:null")
				return true;

			if (nodeEvent is Scalar scalar && scalar.Style == ScalarStyle.Plain)
			{
				var value = scalar.Value;
				return value == "" || value == "~" || value == "null" || value == "Null" || value == "NULL";
			}

			return false;
		}

		private bool IsContainer(Type type)
		{
			if (type == null)
				return false;

			return EntityUtil.mappings.Values.Any(t => t == type);
		}
	}
}
