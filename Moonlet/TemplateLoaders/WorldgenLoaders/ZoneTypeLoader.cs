using HarmonyLib;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using System;
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
		internal int cachedAtlasIndex;

		public int TextureIndex => template.BackgroundIndex;

		public void OnAssetsLoaded()
		{
		}

		public override void Initialize()
		{
			texture = template.Background.LoadTexture<Texture2DArray>(sourceMod, "zonetypes", true, ".dds");

			if (texture == null)
				Issue("Could not load texture for " + id);

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
				Issue($"{template.Border} is not a valid ZoneType reference for borders. Please use one of the following: {Enum.GetNames(typeof(ZoneType)).Join()}");
				template.Border = ZoneType.FrozenWastes.ToString();
			}

			if (!template.Color.hasValue)
			{
				Issue("Has no color defined.");
				template.Color = Color.white;
			}

			if (template.Background == null)
				Issue("Has no background texture defined.");
		}

		public override void RegisterTranslations()
		{
		}
	}
}
