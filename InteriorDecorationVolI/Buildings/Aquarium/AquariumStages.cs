using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class AquariumStages : GameStateMachine<AquariumStages, AquariumStages.Instance>
    {
        public State empty;
        public State delivered;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = empty;

            empty.EventTransition(GameHashes.OccupantChanged, delivered, smi => smi.HasFish());

            delivered.Enter(smi => smi.GetComponent<Aquarium>().AddFish())
                     .EventTransition(GameHashes.OccupantChanged, empty, smi => !smi.HasFish())
                     .Exit(smi => smi.GetComponent<Aquarium>().RemoveFish());
        }

        public new class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master) : base(master) { }

            public bool HasFish()
            {
                var occupant = this.GetComponent<SingleEntityReceptacle>().Occupant;
                return occupant && occupant.HasTag(GameTags.SwimmingCreature);
            }
        }
    }
}
