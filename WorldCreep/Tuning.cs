using System.Collections.Generic;

namespace WorldCreep
{
    public class Tuning
    {
        public const string PREFIX = "WorldCreep_";

        public static HashSet<Tag> worldEventIDs = new HashSet<Tag>()
        {
            WorldEvents.EarthQuakeConfig.ID
        };

        public static HashSet<Tag> meteorIDs = new HashSet<Tag>()
        {
            MushBarConfig.ID
        };
    }
}
