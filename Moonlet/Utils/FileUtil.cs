using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Moonlet.Utils
{
	public class FileUtil
	{
		public static List<(string, T)> ReadYamlsWithPath<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			Log.Debug("GetFiles " + path);

			var list = new List<(string, T)>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				Log.Debug("\t" + file);
				var entry = ReadYaml<T>(file, mappings: mappings);

				if (entry == null)
				{
					Log.Debug($"File {file} was found, but could not be parsed.");
					continue;
				}

				list.Add((file, entry));
			}

			return list;
		}

		public static List<T> ReadYamls<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			Log.Debug("GetFiles " + path);

			var list = new List<T>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				Log.Debug("\t" + file);
				var entry = ReadYaml<T>(file, mappings: mappings);

				if (entry == null)
				{
					Log.Debug($"File {file} was found, but could not be parsed.");
					continue;
				}

				list.Add(entry);
			}

			return list;
		}

		public static T ReadYaml<T>(string path, bool warnIfFailed = false, Dictionary<string, Type> mappings = null) where T : class
		{
			if (!File.Exists(path))
			{
				if (warnIfFailed)
					Log.Warn($"File does not exist {path}");

				return null;
			}

			var builder = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				.IgnoreUnmatchedProperties()
				//.WithNodeTypeResolver(new ComponentTypeResolver())
				.WithNodeDeserializer(new ForceEmptyContainer())
				.IgnoreUnmatchedProperties(str => LogError(str, path));

			mappings?.Do(mapping => builder.WithTagMapping(mapping.Key, mapping.Value));

			var deserializer = builder.Build();

			var content = File.ReadAllText(path);
			return deserializer.Deserialize<T>(content);
		}

		private static void LogError(string str, string path) => Log.Warn($"Error reading {path}: {str}");
	}
}
