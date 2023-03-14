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

        public static HashSet<string> myFacades = new();

        public static void RegisterArtableStages(ArtableStages stages)
        {
            AddBadSculpture(stages, "Bad", FACADES.BROKEN.NAME, FACADES.BROKEN.DESCRIPTION, "decorpacka_glasssculpture_broken_kanim");
            AddMedSculpture(stages, "Average", FACADES.MUCKROOT.NAME, FACADES.MUCKROOT.DESCRIPTION, "decorpacka_glasssculpture_muckroot_kanim");
            AddGreatSculpture(stages, "Good1", FACADES.MEEP.NAME, FACADES.MEEP.DESCRIPTION, "decorpacka_glasssculpture_meep_kanim");
            AddGreatSculpture(stages, "Good2", FACADES.HATCH.NAME, FACADES.HATCH.DESCRIPTION, "decorpacka_glasssculpture_hatch_kanim");
            AddGreatSculpture(stages, "Good3", FACADES.POKESHELL.NAME, FACADES.POKESHELL.DESCRIPTION, "decorpacka_glasssculpture_posh_crab_kanim");
            AddGreatSculpture(stages, "Good4", FACADES.PIP.NAME, FACADES.PIP.DESCRIPTION, "decorpacka_glasssculpture_pip_kanim");
            AddGreatSculpture(stages, "Good5", FACADES.UNICORN.NAME, FACADES.UNICORN.DESCRIPTION, "decorpacka_glasssculpture_unicorn_kanim");
            AddGreatSculpture(stages, "Good6", FACADES.SWAN.NAME, FACADES.SWAN.DESCRIPTION, "decorpacka_glasssculpture_swan_kanim");
            AddGreatSculpture(stages, "Good7", FACADES.GOLEM.NAME, FACADES.GOLEM.DESCRIPTION, "decorpacka_glasssculpture_golem_kanim");
        }

        private static void AddGreatSculpture(ArtableStages stages, string id, string name, string description, string animFile)
        {
            stages.Add(
                "DecorPackA_GlassSculpture_" + id,
                name,
                description,
                PermitRarity.Universal,
                animFile,
                "idle",
                Mod.Settings.GlassSculpture.GeniousSculptureDecorBonus,
                true,
                ArtableStatusType.LookingGreat.ToString(),
                GlassSculptureConfig.ID);
        }

        private static void AddMedSculpture(ArtableStages stages, string id, string name, string description, string animFile)
        {
            stages.Add(
                "DecorPackA_GlassSculpture_" + id,
                name,
                description,
                PermitRarity.Universal,
                animFile,
                "idle",
                Mod.Settings.GlassSculpture.MediocreSculptureDecorBonus,
                false,
                ArtableStatusType.LookingOkay.ToString(),
                GlassSculptureConfig.ID);
        }

        private static void AddBadSculpture(ArtableStages stages, string id, string name, string description, string animFile)
        {
            stages.Add(
                "DecorPackA_GlassSculpture_" + id,
                name,
                description,
                PermitRarity.Universal,
                animFile,
                "idle",
                Mod.Settings.GlassSculpture.BadSculptureDecorBonus,
                false,
                ArtableStatusType.LookingUgly.ToString(),
                GlassSculptureConfig.ID);
        }
    }
}
