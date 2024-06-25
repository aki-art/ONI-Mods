using HarmonyLib;
using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class ArtifactLoader(ArtifactTemplate template, string sourceMod) : EntityLoaderBase<ArtifactTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"UI.SPACEARTIFACTS.{template.Id.ToUpperInvariant()}.{partialKey}";

		public static Dictionary<int, ArtifactTier> tiers = new()
		{
			{ 0, TUNING.DECOR.SPACEARTIFACT.TIER0 },
			{ 1, TUNING.DECOR.SPACEARTIFACT.TIER1 },
			{ 2, TUNING.DECOR.SPACEARTIFACT.TIER2 },
			{ 3, TUNING.DECOR.SPACEARTIFACT.TIER3 },
			{ 4, TUNING.DECOR.SPACEARTIFACT.TIER4 },
			{ 5, TUNING.DECOR.SPACEARTIFACT.TIER5 }
		};

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		protected override GameObject CreatePrefab()
		{
			var tier = TUNING.DECOR.SPACEARTIFACT.TIER_NONE;

			if (!template.Tier.IsNullOrWhiteSpace())
			{
				if (int.TryParse(template.Tier, out var tierNum))
				{
					if (tierNum < 0)
						Error("Lowest possible tier is 0!");
					else if (tierNum > 5)
						Error("Highest possible tier is 5!");
					else
						tier = tiers[tierNum];
				}
				else if (template.Tier != "None")
					Error($"{template.Tier} is not a valid tier. 0, 1, 2, 3, 4, 5 or None");
			}

			var type = ArtifactType.Any;

			if (!template.Type.IsNullOrWhiteSpace() && !Enum.TryParse(template.Type, out type))
				Error($"{template.Type} is not a valid ArtifactType! options are: {Enum.GetNames(typeof(ArtifactType)).Join()}");

			var prefab = ArtifactConfig.CreateArtifact(
				template.Id,
				template.Name,
				template.Description,
				template.Animation.DefaultAnimation,
				"ui",
				tier,
				template.DLC ?? DlcManager.AVAILABLE_ALL_VERSIONS,
				template.Animation.File,
				null, // handled by Component system
				ElementUtil.GetSimhashSafe(template.Element, SimHashes.Creature),
				type);

			ProcessComponents(prefab);

			return prefab;
		}
	}
}
