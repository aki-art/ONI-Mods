using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using System;
using System.IO;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class ZoneTypeLoader(ZoneTypeTemplate template, string sourceMod) : TemplateLoaderBase<ZoneTypeTemplate>(template, sourceMod)
	{
		public ZoneType type;
		public ZoneType borderType;
		public Color32 color32;
		public Texture2DArray texture;
		public string texturesFolder;
		public int runTimeIndex;
		public bool usesAssetBundle;

		public int TextureIndex => template.BackgroundIndex;

		public void OnAssetsLoaded()
		{
			if (template.Background.IsNullOrWhiteSpace())
				Issue("background must be defined.");

			var split = template.Background.Split(FileUtil.delimiter, StringSplitOptions.RemoveEmptyEntries);

			Texture2DArray texture = null;

			if (split.Length > 1)
			{
				var bundleId = split[0];
				var bundle = ModAssets.GetBundle(bundleId);
				if (bundle == null)
				{
					Issue($"Texture for {id} asked for asset bundle {bundleId}, but it has not been loaded. Register the bundle in moonlet_settings.yaml loadAssetBundles");
				}

				texture = bundle.LoadAsset<Texture2DArray>(split[1]);
			}
			else
			{
				texturesFolder = Path.Combine(MoonletMods.Instance.GetAssetsPath(sourceMod, "zonetypes"), template.Background + ".dds");

				if (File.Exists(texturesFolder))
					texture = FileUtil.LoadDdsTexture(texturesFolder, true);
				else
					Issue($"no file found at {texturesFolder}");
			}

			if (texture != null)
			{
				if (texture.width != 1024 || texture.height != 1024)
				{
					Warn($"(debug) {template.Id} texture is not the recommended size. (it is {texture.width}x{texture.height}, recommended is 1024x1024)");
				}
			}
			else
			{
				Issue($"Could not load texture {texturesFolder}");
			}

		}

		public override void Initialize()
		{
			type = ZoneTypeUtil.Register(template);

			color32 = template.Color.value;

			base.Initialize();
		}

		public override void Validate()
		{
			base.Validate();

			if (template.Border.IsNullOrWhiteSpace())
			{
				Issue("Has no border defined.");
				template.Border = ZoneType.FrozenWastes.ToString();
			}

			if (!Enum.TryParse(template.Border, out borderType))
			{
				Issue("{template.Border} is not a valid ZoneType reference for borders.");
				template.Border = ZoneType.FrozenWastes.ToString();
			}

			if (!template.Color.hasValue)
			{
				Issue("Has no color defined.");
				template.Color = Color.white;
			}

			if (template.Background.IsNullOrWhiteSpace())
				Issue("Has no background texture defined.");
		}

		public override void RegisterTranslations()
		{
			AddString($"STRINGS.ZONE_TYPES.{id.LinkAppropiateFormat()}", template.Name);
		}
	}
}
