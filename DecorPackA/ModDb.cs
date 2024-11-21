using Database;
using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using System.Collections.Generic;
using static Database.ArtableStatuses;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE;

namespace DecorPackA
{
	public class ModDb
	{
		public static LampVariants lampVariants;

		public static void Initialize()
		{
			lampVariants = new LampVariants();
		}

		public static HashSet<string> mySculptures = new();

		public static void RegisterArtableStages(ArtableStages stages)
		{
			AddSculpture(stages, "Bad", FACADES.BROKEN.NAME, FACADES.BROKEN.DESCRIPTION, "decorpacka_glasssculpture_broken_kanim", ArtableStatusType.LookingUgly);
			AddSculpture(stages, "Average", FACADES.MUCKROOT.NAME, FACADES.MUCKROOT.DESCRIPTION, "decorpacka_glasssculpture_muckroot_kanim", ArtableStatusType.LookingOkay);
			AddSculpture(stages, "Good1", FACADES.MEEP.NAME, FACADES.MEEP.DESCRIPTION, "decorpacka_glasssculpture_meep_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good2", FACADES.HATCH.NAME, FACADES.HATCH.DESCRIPTION, "decorpacka_glasssculpture_hatch_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good3", FACADES.POKESHELL.NAME, FACADES.POKESHELL.DESCRIPTION, "decorpacka_glasssculpture_posh_crab_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good4", FACADES.PIP.NAME, FACADES.PIP.DESCRIPTION, "decorpacka_glasssculpture_pip_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good5", FACADES.UNICORN.NAME, FACADES.UNICORN.DESCRIPTION, "decorpacka_glasssculpture_unicorn_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good6", FACADES.SWAN.NAME, FACADES.SWAN.DESCRIPTION, "decorpacka_glasssculpture_swan_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Good7", FACADES.GOLEM.NAME, FACADES.GOLEM.DESCRIPTION, "decorpacka_glasssculpture_golem_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "Hound", FACADES.HOUND.NAME, FACADES.HOUND.DESCRIPTION, "decorpacka_glasssculpture_hound_kanim", ArtableStatusType.LookingGreat);
			AddSculpture(stages, "DapperDriller", FACADES.EXCALIBURVOLE.NAME, FACADES.EXCALIBURVOLE.DESCRIPTION, "decorpacka_glasssculpture_dapperdriller_kanim", ArtableStatusType.LookingGreat);
		}

		private static void AddSculpture(ArtableStages stages, string id, string name, string description, string animFile, ArtableStatusType type)
		{
			var bonus = 0;

			switch (type)
			{
				case ArtableStatusType.LookingUgly:
					bonus = 5;
					break;
				case ArtableStatusType.LookingOkay:
					bonus = 10;
					break;
				case ArtableStatusType.LookingGreat:
					bonus = 15;
					break;
			}

			var stageId = "DecorPackA_GlassSculpture_" + id;
			stages.Add(
				stageId,
				name,
				description,
				PermitRarity.Universal,
				animFile,
				"idle",
				bonus,
				type == ArtableStatusType.LookingGreat,
				type.ToString(),
				GlassSculptureConfig.ID,
				string.Empty,
				DlcManager.AVAILABLE_ALL_VERSIONS);

			mySculptures.Add(stageId);
		}
	}
}
