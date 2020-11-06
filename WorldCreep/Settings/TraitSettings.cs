namespace WorldCreep.Settings
{
    public class TraitSettings
    {
        public float AbundantMeteorsMultiplier { get; set; } = 2f;
        public float RareMeteorsMultiplier { get; set; } = 0.5f;
        public bool LushCoreSpawnsKapokTrees { get; set; } = true;
        public bool LostStartUsesModdedBuildings { get; set; } = true;
        public float AsteroidBeltMultiplier { get; set; } = 1.5f;
        public float LonelyPlanetMultiplier { get; set; } = 0.75f;
        public bool LonelyPlanetKeepGuaranteedDestinations { get; set; } = true;
        public bool FrozenWorldUseModdedItems { get; set; } = true;
        public float SuperGeyserMinCount { get; set; } = 1f;
        public float SuperGeyserMaxCount { get; set; } = 2f;
        public float SuperVolcanoMinCount { get; set; } = 1f;
        public float SuperVolcanoMaxCount { get; set; } = 3f;
        public float SeismicUnstabilityMultiplier { get; set; } = 1.66f;
        public float SeismicCalmMultiplier { get; set; } = 0.33f;
        public string[] DisabledTraits { get; set; }
    }
}
