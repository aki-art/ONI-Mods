namespace DecorPackA.Buildings.Aquarium
{
    public class AquariumStages : GameStateMachine<AquariumStages, AquariumStages.Instance>
    {
        public State empty;
        public State ready;
        public State delivered;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = empty;

            //empty
            //    .EventTransition(GameHashes.OnStorageChange, ready, smi => smi.HasWater());

            empty
                .EventTransition(GameHashes.OccupantChanged, delivered, smi => smi.HasFish());

            delivered
                //.DefaultState(delivered.idle)
                .Enter(smi => smi.GetComponent<Aquarium>().AddFish())
                //.EventTransition(GameHashes.OccupantChanged, empty, smi => !smi.HasFish())
                //.EventTransition(GameHashes.OnStorageChange, empty, smi => !smi.HasWater())
                .Exit(smi => smi.GetComponent<Aquarium>().RemoveFish());
        }

        public class DeliveredState : State
        {
            public DeliveredState idle;
            public DeliveredState sad;
            public DeliveredState dead;
        }

        public new class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master) : base(master) { }

            public bool HasFish()
            {
                var occupant = GetComponent<FishReceptable>().Occupant;
                return occupant && occupant.HasTag(GameTags.SwimmingCreature);
            }

            public bool HasWater()
            {
                return GetComponent<Aquarium>().waterStorage.IsFull();
            }
        }
    }
}
