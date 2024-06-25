using Database;
using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using static Database.ArtableStatuses;

namespace Moonlet.TemplateLoaders
{
	public class ArtableLoader(ArtableTemplate template, string sourceMod) : TemplateLoaderBase<ArtableTemplate>(template, sourceMod)
	{
		private static readonly Dictionary<string, ArtableStatusType> statusAliases = new()
		{
			{ "Great", ArtableStatusType.LookingGreat },
			{ "Okay", ArtableStatusType.LookingOkay },
			{ "Ugly", ArtableStatusType.LookingUgly },
			{ "Base", ArtableStatusType.AwaitingArting }
		};

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override string GetTranslationKey(string partialKey)
		{
			return $"STRINGS.BUILDINGS.PREFABS.{template.BuildingId.ToUpperInvariant()}.FACADES.{template.Id.ToUpperInvariant()}.{partialKey}";
		}

		public void LoadContent(ArtableStages stages)
		{
			var statusType = EnumUtils.ParseOrDefault(template.Quality, ArtableStatusType.AwaitingArting, statusAliases, Warn);

			stages.Add(
				$"{template.BuildingId}_{template.Id}",
				template.Name,
				template.Description,
				PermitRarity.Universal,
				template.Animation.File,
				template.Animation.DefaultAnimation,
				template.BonusDecor,
				statusType == ArtableStatusType.LookingGreat,
				statusType.ToString(),
				template.BuildingId,
				"???",
				DlcManager.AVAILABLE_ALL_VERSIONS);
		}
	}
}
