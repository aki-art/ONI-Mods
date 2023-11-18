﻿extern alias YamlDotNetButNew;

using Moonlet.Utils;
using System;
using System.IO;
using UnityEngine;
using YamlDotNetButNew.YamlDotNet.Core;
using YamlDotNetButNew.YamlDotNet.Core.Events;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.SubTemplates
{
	public class TextureEntry : IYamlConvertible
	{
		private string value;

		public TextureType LoadTexture<TextureType>(string sourceMod, string contentPath, bool warnIfNullEntry, string defaultExtension = ".png") where TextureType : Texture
		{
			Log.Debug($"Loading texture {value}");

			if (value == null)
			{
				if (warnIfNullEntry)
					Log.Warn($"Missing texture entry for {contentPath}.", sourceMod);

				return null;
			}

			var split = value.Split(FileUtil.delimiter, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length > 1)
			{
				var bundleId = split[0];

				var bundle = ModAssets.GetBundle(bundleId);
				if (bundle == null)
				{
					Log.Warn($"Texture for {contentPath} {value} is asking for an AssetBundle by the ID {bundleId}, but it doesn't exist.", sourceMod);
					return null;
				}

				var result = bundle.LoadAsset<TextureType>(split[1]);
				if (result == null)
					Log.Warn($"AssetBundle {bundleId} has no texture at path {split[1]}.", sourceMod);

				return result;
			}
			else if (split.Length == 1)
			{
				var absolutePath = MoonletMods.Instance.GetAssetsPath(sourceMod, contentPath);
				absolutePath = Path.Combine(absolutePath, value);

				if (!Path.HasExtension(absolutePath))
					absolutePath += defaultExtension;

				if (!FileUtil.Exists(absolutePath))
				{
					Log.Warn($"Texture for {contentPath} {value} is not found at {absolutePath}.", sourceMod);
					return null;
				}

				if (typeof(TextureType) == typeof(Texture2DArray))
				{
					var tex = FileUtil.LoadDdsTexture(absolutePath, true);
					if ((tex == null))
					{
						Log.Warn("null texture");
					}
					return (TextureType)(Texture)FileUtil.LoadDdsTexture(absolutePath, true);
				}
				else
					return (TextureType)(Texture)FUtility.Assets.LoadTexture(absolutePath);
			}

			Log.Warn($"Texture for {contentPath} {value} is not a valid format.\n" +
				$"<localTexturePath> to define a relative path in the assets folder.\n" +
				$"<assetBundleId::assetPath> to define a path in a registered asset bundle.", sourceMod);

			return null;
		}

		public TextureEntry(string value) : this()
		{
			this.value = value;
		}

		public TextureEntry()
		{

		}

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			if (parser.TryConsume<Scalar>(out var ev))
			{
				value = ev.Value;
			}
			else
			{
				var values = (TextureEntry)nestedObjectDeserializer(typeof(TextureEntry));
				value = values.value;
			}
		}

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			nestedObjectSerializer(new TextureEntry(value));
		}
	}
}
