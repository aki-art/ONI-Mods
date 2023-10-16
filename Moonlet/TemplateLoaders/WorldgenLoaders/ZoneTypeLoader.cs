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

		public override void Initialize()
		{
			type = ZoneTypeUtil.Register(template);

			color32 = template.Color.value;
			texturesFolder = Path.Combine(MoonletMods.Instance.GetAssetsPath(sourceMod, "zonetypes"), template.Background + ".dds");

			if (File.Exists(texturesFolder))
				texture = FileUtil.LoadDdsTexture(texturesFolder, true);
			else
				Warn($"{template.Id}: no file found at {texturesFolder}");

			if (texture.width != 1024 || texture.height != 1024)
			{
				Warn($"(debug) {template.Id} texture is not the recommended size. (it is {texture.width}x{texture.height}, recommended is 1024x1024)");
			}

			base.Initialize();
		}

		public override void Validate()
		{
			base.Validate();

			if (template.Border.IsNullOrWhiteSpace())
			{
				Warn($"{template.Id}: Has no border defined.");
				template.Border = ZoneType.FrozenWastes.ToString();
			}

			if (!Enum.TryParse(template.Border, out borderType))
			{
				Warn($"{template.Id}: {template.Border} is not a valid ZoneType reference for borders.");
				template.Border = ZoneType.FrozenWastes.ToString();
			}

			if (!template.Color.hasValue)
			{
				Warn($"{template.Id}: Has no color defined.");
				template.Color = Color.white;
			}

			if (template.Background.IsNullOrWhiteSpace())
				Warn($"{template.Id}: Has no background texture defined.");
		}

		public override void RegisterTranslations()
		{
			AddString($"STRINGS.ZONE_TYPES.{id.LinkAppropiateFormat()}", template.Name);
		}
	}
}
