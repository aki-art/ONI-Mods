using Database;
using DecorPackB.Content.Buildings;
using static Database.ArtableStatuses;

namespace DecorPackB.Content
{
    public class DPArtableStages
    {
        public static void Register(ArtableStages stages)
        {
            // smaller fossil
            AddStage(stages, FossilDisplayConfig.ID, "human", "decorpackb_fossildisplay_human_kanim", 5, ArtableStatusType.LookingUgly);
            AddStage(stages, FossilDisplayConfig.ID, "pacu", "decorpackb_fossildisplay_pacu_kanim", 10, ArtableStatusType.LookingOkay);
            AddStage(stages, FossilDisplayConfig.ID, "dodo", "decorpackb_fossildisplay_dodo_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "beefalo", "decorpackb_fossildisplay_beefalo_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "minipara", "decorpackb_fossildisplay_minipara_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "trilobite", "decorpackb_fossildisplay_trilobite_kanim", 15, ArtableStatusType.LookingGreat);

            // giant fossil
            AddStage(stages, GiantFossilDisplayConfig.ID, "trex", "decorpackb_giantfossil_trex_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, GiantFossilDisplayConfig.ID, "livayatan", "decorpackb_giantfossil_livayatan_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, GiantFossilDisplayConfig.ID, "bronto", "decorpackb_giantfossil_bronto_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, GiantFossilDisplayConfig.ID, "triceratops", "decorpackb_giantfossil_triceratops_kanim", 15, ArtableStatusType.LookingGreat);

            // pot
            AddStage(stages, PotConfig.ID, "hatch", "decorpackb_pot_hatch_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "morb", "decorpackb_pot_morb_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "swirlies", "decorpackb_pot_swirlies_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "swirlies_purple", "decorpackb_pot_swirlies_purple_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "swirlies_bluegold", "decorpackb_pot_swirlies_bluegold_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "pinkyfluffylettuce", "decorpackb_pot_pinkyfluffylettuce_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "angrylettuce", "decorpackb_pot_angrylettuce_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "generic_tall", "decorpackb_pot_generic_tall_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "muckroot", "decorpackb_pot_muckroot_kanim", 15, ArtableStatusType.LookingGreat, "off");
            AddStage(stages, PotConfig.ID, "rectangular", "decorpackb_pot_rectangular_kanim", 5, ArtableStatusType.LookingUgly, "off");
            AddStage(stages, PotConfig.ID, "weird", "decorpackb_pot_weird_kanim", 10, ArtableStatusType.LookingOkay, "off");
            AddStage(stages, PotConfig.ID, "red", "decorpackb_pot_red_kanim", 10, ArtableStatusType.LookingOkay, "off");
        }

        private static string AddStage(ArtableStages stages, string buildingID, string ID, string anim, int decorBonus, ArtableStatusType status, string defaultAnim = "idle")
        {
            var key = $"DecorPackB.STRINGS.BUILDINGS.PREFABS.{buildingID.ToUpperInvariant()}.VARIANT.{ID.ToUpperInvariant()}";
            var id = $"DecorPackB_{buildingID}_{ID}";
            var name = Strings.Get(key + ".NAME");
            var description = Strings.Get(key + ".DESCRIPTION");

            PermitResources.PermitIdsToExcludeFromSupplyCloset.Add(id);

            stages.Add(
                $"DecorPackB_{buildingID}_{ID}",
                name,
                description,
                PermitRarity.Universal,
                anim,
                defaultAnim,
                decorBonus,
                status == ArtableStatusType.LookingGreat,
                status.ToString(),
                buildingID);

            return id;
        }
    }
}
