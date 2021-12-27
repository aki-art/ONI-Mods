using SchwartzRocketEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchwartzRocketEngine
{
    internal class ModAssets
    {
        // TODO: init this on all mods loaded
        public static Dictionary<string, List<SiftResultOption>> siftRewards = new Dictionary<string, List<SiftResultOption>>()
        {
            {
                SimHashes.Sand.CreateTag().ToString(),
                new List<SiftResultOption>()
                {
                    new SiftResultOption(SimHashes.GoldAmalgam.CreateTag(), 50, 1),
                    new SiftResultOption(SimHashes.Cuprite.CreateTag(), 50, 1),
                    new SiftResultOption(BasicForagePlantConfig.ID, 1, 2)
                }
            },
            {
                SimHashes.Regolith.CreateTag().ToString(),
                new List<SiftResultOption>()
                {
                    new SiftResultOption(SimHashes.Iron.CreateTag(), 50, 1),
                    new SiftResultOption(SimHashes.Lead.CreateTag(), 50, 1),
                    new SiftResultOption(MoleConfig.EGG_ID, 1, 2)
                }
            }
        };
    }
}
