using FUtility;
using System;
using System.Collections.Generic;
using System.Text;
using Terraformer.Entities;

namespace Terraformer
{
    public class InstantDestroyablePlanet : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpGet]
        public WorldContainer worldContainer;

        StringBuilder conditionMsg;

        private bool targetable;

        private static List<Condition> conditions = new List<Condition>()
        {
            new Condition(world => !world.IsStartWorld, STRINGS.UI.WORLD_DESTRUCTION.STARTING_WORLD),
            new Condition(world => world.IsDiscovered, STRINGS.UI.WORLD_DESTRUCTION.DISCOVERED),
            new Condition(world => !Util.AreAnySpaceCraftsPresent(world), STRINGS.UI.WORLD_DESTRUCTION.ROCKET_NOT_PRESENT),
            //new Condition(world => Mod.WorldDestroyers.GetWorldItems(world.GetMyWorldId()).Count == 0, "Already Destroyed")
        };

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit(); 
            conditionMsg = new StringBuilder();
        }

        public class Condition
        {
            public Predicate<WorldContainer> predicate;
            public string description;

            public Condition(Predicate<WorldContainer> predicate, string description)
            {
                this.predicate = predicate;
                this.description = description;
            }
        }

        public string SidescreenButtonText => "Dump Grid data";

        public string SidescreenButtonTooltip => "";

        public bool IsWorldTargetable()
        {
            targetable = true;
            conditionMsg.Clear();

            if (worldContainer is null)
            {
                Log.Warning("Trying to target world, but it's container is NULL");
                return false;
            }

            foreach (var condition in conditions)
            {
                if(!condition.predicate(worldContainer))
                {
                    targetable = false;
                    conditionMsg.Append(STRINGS.FormatAsBad(condition.description)).Append("\n");
                }
                else
                {
                    conditionMsg.Append(STRINGS.FormatAsGood(condition.description)).Append("\n");
                }
            }

            Log.Debuglog(conditionMsg.ToString());

            Trigger((int)ModHashes.SidescreenRefresh);
            return targetable;
        }

        public string GetText()
        {
            return conditionMsg.ToString();
        }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            Util.DumpGrid();
        }

        public int ButtonSideScreenSortOrder() => 0;
    }
}
