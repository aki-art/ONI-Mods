using Database;
using DecorPackB.Content.Defs.Buildings;
using System.Collections.Generic;
using static Database.ArtableStatuses;

namespace DecorPackB.Content.ModDb
{
	public class DPArtableStages
	{
		public static HashSet<string> hangables;
		public static HashSet<string> fossils = new();
		public static HashSet<string> pots = new();
		public static HashSet<string> fountains = new();

		public static void Register(ArtableStages stages)
		{
			// smaller fossil
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "human", "decorpackb_fossildisplay_human_kanim", 5, ArtableStatusType.LookingUgly));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "doggy", "decorpackb_fossildisplay_doggy_kanim", 5, ArtableStatusType.LookingUgly));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "pacu", "decorpackb_fossildisplay_pacu_kanim", 10, ArtableStatusType.LookingOkay));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "ammonite", "decorpackb_fossildisplay_ammonite_kanim", 10, ArtableStatusType.LookingOkay));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "pawprints", "decorpackb_fossildisplay_pawprints_kanim", 10, ArtableStatusType.LookingOkay));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "ancientspecimenamber", "decorpackb_fossildisplay_ancientspecimentamber_kanim", 10, ArtableStatusType.LookingOkay));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "dodo", "decorpackb_fossildisplay_dodo_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "beefalo", "decorpackb_fossildisplay_beefalo_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "minipara", "decorpackb_fossildisplay_minipara_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "trilobite", "decorpackb_fossildisplay_trilobite_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "microraptor", "decorpackb_fossildisplay_microraptor_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "ancientspeciment", "decorpackb_fossildisplay_ancientspecimen_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, FossilDisplayConfig.ID, "glommer", "decorpackb_fossildisplay_glommer_kanim", 15, ArtableStatusType.LookingGreat));

			// giant fossil
			fossils.Add(ArtableUtil.AddStage(stages, GiantFossilDisplayConfig.ID, "trex", "decorpackb_giantfossil_trex_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, GiantFossilDisplayConfig.ID, "livayatan", "decorpackb_giantfossil_livayatan_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, GiantFossilDisplayConfig.ID, "bronto", "decorpackb_giantfossil_bronto_kanim", 15, ArtableStatusType.LookingGreat));
			fossils.Add(ArtableUtil.AddStage(stages, GiantFossilDisplayConfig.ID, "triceratops", "decorpackb_giantfossil_triceratops_kanim", 15, ArtableStatusType.LookingGreat));

			// pot
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "hatch", "decorpackb_pot_hatch_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "morb", "decorpackb_pot_morb_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "swirlies", "decorpackb_pot_swirlies_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "swirlies_purple", "decorpackb_pot_swirlies_purple_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "swirlies_bluegold", "decorpackb_pot_swirlies_bluegold_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "pinkyfluffylettuce", "decorpackb_pot_pinkyfluffylettuce_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "angrylettuce", "decorpackb_pot_angrylettuce_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "generic_tall", "decorpackb_pot_generic_tall_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "muckroot", "decorpackb_pot_muckroot_kanim", 15, ArtableStatusType.LookingGreat, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "rectangular", "decorpackb_pot_rectangular_kanim", 5, ArtableStatusType.LookingUgly, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "weird", "decorpackb_pot_weird_kanim", 10, ArtableStatusType.LookingOkay, "off"));
			pots.Add(ArtableUtil.AddStage(stages, PotConfig.ID, "red", "decorpackb_pot_red_kanim", 10, ArtableStatusType.LookingOkay, "off"));

			// fountains
			fountains.Add(ArtableUtil.AddStage(stages, FountainConfig.ID, "fish", "decorpackb_fountain_fish_kanim", 15, ArtableStatusType.LookingGreat));
			fountains.Add(ArtableUtil.AddStage(stages, FountainConfig.ID, "bowls", "decorpackb_fountain_bowls_kanim", 10, ArtableStatusType.LookingOkay));

			hangables = new HashSet<string>()
			{
				"livayatan"
			};
		}
	}
}
