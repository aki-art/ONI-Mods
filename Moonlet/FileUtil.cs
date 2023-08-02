using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Moonlet
{
	public class FileUtil
	{
		public static T Read<T>(string path, bool warnIfFailed = false, Dictionary<string, Type> mappings = null) where T : class
		{
			if (!File.Exists(path))
			{
				if (warnIfFailed)
					Log.Warning($"File does not exist {path}");

				return null;
			}

			var builder = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				//.WithNodeTypeResolver(new ComponentTypeResolver())
				.WithNodeDeserializer(new ForceEmptyContainer())
				.IgnoreUnmatchedProperties(str => LogError(str, path));

			mappings?.Do(mapping => builder.WithTagMapping(mapping.Key, mapping.Value));

			var deserializer = builder.Build();

			var content = File.ReadAllText(path);
			return deserializer.Deserialize<T>(content);
		}

		private static void LogError(string str, string path)
		{
			Log.Warning($"Error reading {path}: {str}");
		}
	}
}
