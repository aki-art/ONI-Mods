namespace DecorPackB.Content
{
    public class DPTags
    {
        public static Tag liteFossilMaterial = TagManager.Create("DecorPackB_LiteFossilMaterial");
        public static Tag trueFossilMaterial = TagManager.Create("DecorPackB_TrueFossilMaterial");

        // For rooms expanded, it uses this to recognize fossil buildings
        public static readonly Tag FossilBuilding = TagManager.Create("FossilBuilding");

        // items need a special tag that is marked as "buildable material" for the game
        public static readonly Tag FossilNodule = TagManager.Create(Mod.PREFIX + "FossilNodule");

        public static readonly Tag DigYieldModifier = TagManager.Create(Mod.PREFIX + "DigYieldModifier");
    }
}
