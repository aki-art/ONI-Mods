namespace WorldTraitsPlus.WorldEvents
{
    class SeismicOverlayMode
    {
        public class SeismicMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "SeismicOverlay";
            public override string GetSoundName() => "Decor";
        }

        public class FaultChunksOverlayMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "SeismicChunksOverlay";
            public override string GetSoundName() => "Decor";
        }
        public class EarthQuakeOverlayMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "EarthquakeOverlay";
            public override string GetSoundName() => "Decor";
        }
    }
}
