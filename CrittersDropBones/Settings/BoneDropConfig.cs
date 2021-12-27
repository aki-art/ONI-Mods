using CrittersDropBones.Items;
using FUtility.SaveData;
using System.Collections.Generic;

namespace CrittersDropBones.Settings
{
    public class Config : IUserSetting
    {
        public class BoneDrops
        {
            public List<BoneDropConfig> Drops { get; set; } = new List<BoneDropConfig>();

            public BoneDrops Add(string drop, float amount = 1)
            {
                Drops.Add(new BoneDropConfig(drop, amount));
                return this;
            }
        }

        public class BoneDropConfig
        {
            public string Drop { get; set; }

            public float Amount { get; set; }

            public BoneDropConfig(string drop, float amount = 1)
            {
                Drop = drop;
                Amount = amount;
            }
        }

        public Dictionary<string, BoneDrops> Bones { get; set; } = new Dictionary<string, BoneDrops>()
        {
            // Vanilla
            //Pacus
            { PacuConfig.ID, new BoneDrops().Add(FishBoneConfig.ID) },
            { PacuTropicalConfig.ID, new BoneDrops().Add(FishBoneConfig.ID) },
            { PacuCleanerConfig.ID, new BoneDrops().Add(FishBoneConfig.ID) },

            // Pufts
            { PuftConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },
            { PuftBleachstoneConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },
            { PuftAlphaConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },
            { PuftOxyliteConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },

            // Pip
            { SquirrelConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },

            // Shovole
            { MoleConfig.ID, new BoneDrops().Add(MediumBoneConfig.ID) },

            // Dreckos
            { DreckoConfig.ID, new BoneDrops().Add(LargeBoneConfig.ID) },
            { DreckoPlasticConfig.ID, new BoneDrops().Add(LargeBoneConfig.ID) },

            // Moo
            { MooConfig.ID, new BoneDrops().Add(LargeBoneConfig.ID) },

            // Pacu Morphs
            { "PacuPlate", new BoneDrops().Add(FishBoneConfig.ID) },
            { "PacuAlgae", new BoneDrops().Add(FishBoneConfig.ID) },
            { "PacuBeta", new BoneDrops().Add(FishBoneConfig.ID) },

            // Squirrel Morphs
            { "SquirrelSpring", new BoneDrops().Add(MediumBoneConfig.ID) },
            { "SquirrelAutumn", new BoneDrops().Add(MediumBoneConfig.ID) },
            { "SquirrelWinter", new BoneDrops().Add(MediumBoneConfig.ID) },

            // Puft Morphs
            { "PuftDevil", new BoneDrops().Add(MediumBoneConfig.ID) },
            { "PuftHydrogen", new BoneDrops().Add(MediumBoneConfig.ID) },
            { "PuftCO2", new BoneDrops().Add(MediumBoneConfig.ID) },

            // Drecko Morphs
            { "DreckoOpulent", new BoneDrops().Add(LargeBoneConfig.ID) },
            { "DreckoGlass", new BoneDrops().Add(LargeBoneConfig.ID) },
            { "DreckoMossy", new BoneDrops().Add(LargeBoneConfig.ID) }
        };
    }
}
