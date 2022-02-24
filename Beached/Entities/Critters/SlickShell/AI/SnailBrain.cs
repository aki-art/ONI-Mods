using UnityEngine;

namespace Beached.Entities.Critters.SlickShell.AI
{
    public class SnailBrain
    {
        public static void ConfigureAI(GameObject gameObject, string symbolOverridePrefix, Tag species)
        {
            var choreTable = new ChoreTable.Builder()
                .Add(new DeathStates.Def())
                .Add(new AnimInterruptStates.Def())
                //.Add(new GrowUpStates.Def())
                .Add(new TrappedStates.Def())
                //.Add(new IncubatingStates.Def())
                .Add(new BaggedStates.Def())
                .Add(new FallStates.Def())
                //.Add(new StunnedStates.Def())
                .Add(new DebugGoToStates.Def())
                .Add(new FleeStates.Def())
                //.Add(new DefendStates.Def())
                //.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
                .PushInterruptGroup()
                //.Add(new CreatureSleepStates.Def())
                .Add(new MucusSecretionStates.Def())
                .Add(new FixedCaptureStates.Def())
                //.Add(new RanchedStates.Def())
                //.Add(new LayEggStates.Def())
                .Add(new EatStates.Def())
                //.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
                .Add(new CallAdultStates.Def())
                .PopInterruptGroup()
                .Add(new IdleStates.Def());

            EntityTemplates.AddCreatureBrain(gameObject, choreTable, species, symbolOverridePrefix);
        }
    }
}
