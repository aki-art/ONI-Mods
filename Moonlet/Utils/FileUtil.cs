extern alias YamlDotNetButNew;
using Klei;
using Moonlet.Scripts.ComponentTypes;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNetButNew.YamlDotNet.Serialization;
using YamlDotNetButNew.YamlDotNet.Serialization.NamingConventions;
using Path = System.IO.Path;

namespace Moonlet.Utils
{
	public class FileUtil
	{
		public static readonly string[] delimiter = ["::"];

		public static List<(string, T)> ReadYamlsWithPath<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			var list = new List<(string, T)>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
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

		/// <see cref="SettingsCache.LoadTrait(FileHandle, string, string, Dictionary{string, WorldTrait}, List{YamlIO.Error})"/>
		public static string GetRelativePathKleiWay(string directory, string parentDirectory)
		{
			directory = FileSystem.Normalize(directory);
			parentDirectory = FileSystem.Normalize(parentDirectory);

			var length = SettingsCache.FirstUncommonCharacter(parentDirectory, directory);
			var result = (length > -1) ? directory.Substring(length) : directory;

			result = Path.Combine(Path.GetDirectoryName(result), Path.GetFileNameWithoutExtension(result));
			result = result.Replace('\\', '/');

			return result;
		}

		public static List<T> ReadYamls<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			var list = new List<T>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
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
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.IncludeNonPublicProperties()
				//.WithNodeTypeResolver(new ComponentTypeResolver())

				//.WithNodeDeserializer(new ForceEmptyContainer())
				.IgnoreUnmatchedProperties();

			// working original example
			/*			builder.WithTypeDiscriminatingNodeDeserializer((o) =>
						{
							IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
								{
									{ "Type1", typeof(Type1) },
									{ "Type2", typeof(Type2) }
								};
							o.AddKeyValueTypeDiscriminator<TypeObject>("objectType", valueMappings); // "ObjectType" must match the name of the key exactly as it appears in the Yaml document.
						});*/

			builder.WithTypeDiscriminatingNodeDeserializer((o) =>
			{
				IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
					{
						{ "Edible", typeof(EdibleComponent) }
					};
				o.AddKeyValueTypeDiscriminator<BaseComponent>("type", valueMappings); // "ObjectType" must match the name of the key exactly as it appears in the Yaml document.
			});

			if (mappings != null)
			{

				/*	builder.WithTypeDiscriminatingNodeDeserializer((o) =>
					{
						IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
						{
										{ "Edible", typeof(FEdibleComponent) },
										{ "Type2", typeof(Type2) }
						};
						o.AddKeyValueTypeDiscriminator<FBaseComponent>("type", valueMappings); // "ObjectType" must match the name of the key exactly as it appears in the Yaml document.
					});*/
				/*builder.WithTypeDiscriminatingNodeDeserializer((o) =>
				{
					IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
				{
					{ "edible", typeof(EdibleComponent) }
				};
					o.AddKeyValueTypeDiscriminator<BaseComponent>("type", valueMappings); // "ObjectType" must match the name of the key exactly as it appears in the Yaml document.
				});*/
			}
			/*
						if (mappings != null)
						{
							Log.Debug("adding mappings");
							builder.WithTypeDiscriminatingNodeDeserializer(o =>
							{
								o.AddKeyValueTypeDiscriminator<BaseComponent>("type", mappings);
							});
						}*/

			//mappings?.Do(mapping => builder.WithTagMapping(mapping.Key, mapping.Value));

			var deserializer = builder.Build();

			var content = File.ReadAllText(path);

			if (content == null)
			{
				Log.Warn($"File could not be read {path}");
				return null;
			}

			return deserializer.Deserialize<T>(content);
		}


		public static string GetOrCreateDirectory(string path)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				return path;
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warn("Could not create directory: " + e.Message);
				return null;
			}
		}

		public static Texture2DArray LoadDdsTexture(string path, bool warnIfFailed = true)
		{
			Texture2DArray texture = null;

			if (File.Exists(path))
			{
				var data = FUtility.Assets.TryReadFile(path);
				var tex2D = LoadTextureDXT(data, TextureFormat.DXT1);

				texture = new Texture2DArray(1024, 1024, 1, TextureFormat.DXT1, false);
				texture.SetPixelData(tex2D.GetPixelData<byte>(0), 0, 0);
				texture.Apply();
			}
			else if (warnIfFailed)
			{
				Log.Warn($"Could not load texture at path {path}.");
			}

			return texture;
		}

		// credit: https://discussions.unity.com/t/can-you-load-dds-textures-during-runtime/84192/2

		public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
		{
			if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
				throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

			byte ddsSizeCheck = ddsBytes[4];
			if (ddsSizeCheck != 124)
				throw new Exception("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

			int height = ddsBytes[13] * 256 + ddsBytes[12];
			int width = ddsBytes[17] * 256 + ddsBytes[16];

			int DDS_HEADER_SIZE = 128;
			byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
			Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

			Texture2D texture = new Texture2D(width, height, textureFormat, false);
			texture.LoadRawTextureData(dxtBytes);
			texture.Apply();

			return (texture);
		}

		private static void LogError(string str, string path) => Log.Warn($"Error reading {path}: {str}");

		public static void WriteYAML(string path, object obj)
		{
			YamlIO.Save(obj, path);
		}

		internal static bool TryReadBasicYaml<T>(string path, out T result)
		{
			if (!File.Exists(path))
			{
				result = default;
				return false;
			}

			var builder = new DeserializerBuilder()
				.IncludeNonPublicProperties()
				//.WithNodeDeserializer(new ForceEmptyContainer())
				.IgnoreUnmatchedProperties();

			var deserializer = builder.Build();

			var content = File.ReadAllText(path);
			result = deserializer.Deserialize<T>(content);
			return true;
		}

		public static string GetPlatformString()
		{
			return Application.platform switch
			{
				RuntimePlatform.WindowsPlayer => "windows",
				RuntimePlatform.LinuxPlayer => "linux",
				RuntimePlatform.OSXPlayer => "mac",
				_ => "",
			};
		}

		public static bool Exists(string absolutePath)
		{
			// TODO: Case sensitive
			return File.Exists(absolutePath);
		}
	}
}
