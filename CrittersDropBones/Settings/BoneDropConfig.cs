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
            { PuftConfig.ID, new BoneDrops().Add(BoneConfig.ID) },
            { PuftBleachstoneConfig.ID, new BoneDrops().Add(BoneConfig.ID) },
            { PuftAlphaConfig.ID, new BoneDrops().Add(BoneConfig.ID) },
            { PuftOxyliteConfig.ID, new BoneDrops().Add(BoneConfig.ID) },

            // Pip
            { SquirrelConfig.ID, new BoneDrops().Add(BoneConfig.ID) },

            // Shovole
            { MoleConfig.ID, new BoneDrops().Add(BoneConfig.ID) },

            // Dreckos
            { DreckoConfig.ID, new BoneDrops().Add(BoneConfig.ID, 10) },
            { DreckoPlasticConfig.ID, new BoneDrops().Add(BoneConfig.ID, 10) },

            // Moo
            { MooConfig.ID, new BoneDrops().Add(BoneConfig.ID, 12) },

            // Pacu Morphs
            { "PacuPlate", new BoneDrops().Add(FishBoneConfig.ID) },
            { "PacuAlgae", new BoneDrops().Add(FishBoneConfig.ID) },
            { "PacuBeta", new BoneDrops().Add(FishBoneConfig.ID) },

            // Squirrel Morphs
            { "SquirrelSpring", new BoneDrops().Add(BoneConfig.ID) },
            { "SquirrelAutumn", new BoneDrops().Add(BoneConfig.ID) },
            { "SquirrelWinter", new BoneDrops().Add(BoneConfig.ID) },

            // Puft Morphs
            { "PuftDevil", new BoneDrops().Add(BoneConfig.ID) },
            { "PuftHydrogen", new BoneDrops().Add(BoneConfig.ID) },
            { "PuftCO2", new BoneDrops().Add(BoneConfig.ID) },

            // Drecko Morphs
            { "DreckoOpulent", new BoneDrops().Add(BoneConfig.ID, 10) },
            { "DreckoGlass", new BoneDrops().Add(BoneConfig.ID, 10) },
            { "DreckoMossy", new BoneDrops().Add(BoneConfig.ID, 10) }
        };
    }
}
