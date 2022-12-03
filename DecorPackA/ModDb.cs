using Database;
using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
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

        public static void RegisterArtableStages(ArtableStages stages)
        {
            var config = Mod.Settings.GlassSculpture;
            var anim = "sculpture_glass_kanim";
            var artableStatuses = Db.Get().ArtableStatuses;
            var ID = GlassSculptureConfig.ID;

            stages.Add("DecorPackA_GlassSculpture_Bad", FACADES.BROKEN.NAME, FACADES.BROKEN.DESCRIPTION, PermitRarity.Universal, anim, "crap_1", config.BadSculptureDecorBonus, false, ArtableStatusType.LookingUgly.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Average", FACADES.MUCKROOT.NAME, FACADES.MUCKROOT.DESCRIPTION, PermitRarity.Universal, anim, "good_1", config.MediocreSculptureDecorBonus, false, ArtableStatusType.LookingOkay.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good1", FACADES.MEEP.NAME, FACADES.MEEP.DESCRIPTION, PermitRarity.Universal, anim, "amazing_1", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good2", FACADES.HATCH.NAME, FACADES.HATCH.DESCRIPTION, PermitRarity.Universal, anim, "amazing_2", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good3", FACADES.POKESHELL.NAME, FACADES.POKESHELL.DESCRIPTION, PermitRarity.Universal, anim, "amazing_3", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good4", FACADES.PIP.NAME, FACADES.PIP.DESCRIPTION, PermitRarity.Universal, anim, "amazing_4", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good5", FACADES.UNICORN.NAME, FACADES.UNICORN.DESCRIPTION, PermitRarity.Universal, anim, "amazing_5", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good6", FACADES.SWAN.NAME, FACADES.SWAN.DESCRIPTION, PermitRarity.Universal, anim, "amazing_6", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
            stages.Add("DecorPackA_GlassSculpture_Good7", FACADES.GOLEM.NAME, FACADES.GOLEM.DESCRIPTION, PermitRarity.Universal, anim, "amazing_7", config.GeniousSculptureDecorBonus, true, ArtableStatusType.LookingGreat.ToString(), ID);
        }
    }
}
