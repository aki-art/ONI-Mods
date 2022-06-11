using FUtility.SaveData;
using System;
using System.Collections.Generic;

namespace DecorPackB.Settings
{
    public class Config : IUserSetting
    {
        public ArcheologyBonusConfig Archeology { get; set; } = new ArcheologyBonusConfig();

        [Serializable]
        public class ArcheologyBonusConfig
        {
            public float BonusMaterialPercent { get; set; } = 0.15f;

            public float ChanceOfBonus { get; set; } = 1f;
        }

        public Dictionary<SimHashes, TreasureChances> TreasureHunterLoots { get; set; } = new Dictionary<SimHashes, TreasureChances>()
        {
            {
                SimHashes.SandStone,
                new TreasureChances()
                {
                    extraLootChance = 0.1f,
                    treasures = new List<TreasureChances.Treasure>()
                    {
                        new TreasureChances.Treasure(PrickleFlowerConfig.SEED_ID, 1f, 1f),
                        new TreasureChances.Treasure(EggShellConfig.ID,  0.25f, 1f),
                    }
                }
            },
            {
                SimHashes.MaficRock,
                new TreasureChances()
                {
                    extraLootChance = 0.5f,
                    treasures = new List<TreasureChances.Treasure>()
                    {
                        new TreasureChances.Treasure(PrickleFlowerConfig.SEED_ID, 1f, 1f),
                        new TreasureChances.Treasure(EggShellConfig.ID,  0.25f, 1f),
                    }
                }
            },
        };
    }
}
