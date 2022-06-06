using Database;
using FUtility;
using ProcGen;
using System;
using System.Collections.Generic;

namespace DecorPackB
{
    public class TreasureChances
    {
        public List<Treasure> treasures;

        public void OnExcavation(Diggable diggable, Element element, MinionResume minion)
        {
            var item = treasures.GetWeightedRandom();

            if(item != null )
            {
                Utils.Spawn(item.tag, diggable.gameObject);
            }
        }

        public class Treasure : IWeighted
        {
            public Tag tag;

            public float weight { get; set; }
        }
    }
}
