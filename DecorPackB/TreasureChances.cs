using Database;
using FUtility;
using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB
{
    public class TreasureChances
    {
        public List<Treasure> treasures;
        public float extraLootChance;
        public float minimumMassKg = 300f;

        public void OnExcavation(Diggable diggable, int cell, Element element, MinionResume minion)
        {
            var mass = Grid.Mass[cell];

            if(mass < minimumMassKg)
            {
                return;
            }

            if(extraLootChance < UnityEngine.Random.value)
            {
                return;
            }

            var item = treasures.GetWeightedRandom();

            if(item != null)
            {
                var loot = Utils.Spawn(item.tag, diggable.gameObject);

                var primaryElement = loot.GetComponent<PrimaryElement>();
                primaryElement.Mass = item.amount;
                primaryElement.Temperature = Grid.Temperature[cell];
                // TODO: disease

                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, loot.GetProperName(), loot.transform);
            }
        }

        [Serializable]
        public class Treasure : IWeighted
        {
            public string tag;

            public float amount { get; set; }

            public float weight { get; set; }

            public Treasure(string tag, float amount, float weight)
            {
                this.tag = tag;
                this.weight = weight;
                this.amount = amount;
            }
        }
    }
}
