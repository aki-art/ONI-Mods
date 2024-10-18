extern alias YamlDotNetButNew;
using System;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;
using YamlDotNetButNew.YamlDotNet.Serialization.ObjectFactories;

namespace Moonlet
{
	// makes sure that Commands and Components can be empty, so a single type definition works with a null object defined, and just assumes default values
	// https://github.com/aaubry/YamlDotNet/issues/443#issuecomment-544449498
	public sealed class ForceEmptyContainer : INodeDeserializer
	{
		private readonly IObjectFactory objectFactory = new DefaultObjectFactory();

		public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value, ObjectDeserializer rootDeserializer)
		{
			value = null;

			if (IsContainer(expectedType) && reader.TryConsume<NodeEvent>(out var ev))
			{
				Log.Debug(ev.Tag);
				Log.Debug("node consumed");
				if (NodeIsNull(ev))
				{
					Log.Debug("null node!!");
					reader.SkipThisAndNestedEvents();
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
				Log.Debug("node value is " + value);
				return value == "" || value == "~" || value == "null" || value == "Null" || value == "NULL";
			}
			else if (nodeEvent is MappingStart mappingStart)
			{
				Log.Debug($"start:{mappingStart.Start}");
				Log.Debug($"end:{mappingStart.End}");
				Log.Debug($"style:{mappingStart.Style}");
			}
			else
			{
				Log.Debug($"nodevent is not scalar {nodeEvent.GetType().Name}");
				if (nodeEvent is Scalar scalar2)
				{
					Log.Debug($"scalar style: {scalar2.Style}");
				}
			}

			return false;
		}

		private bool IsContainer(Type type)
		{
			if (type == null)
				return false;

			Log.Debug($"checking if container type {type.Name} {Mod.componentTypes.ContainsValue(type)}");
			return Mod.componentTypes.ContainsValue(type); //EntityUtil.mappings.Values.Any(t => t == type);
		}
	}
}
