using System;

namespace Asphalt
{
    [Serializable]
    public class TempSettings
    {
        public bool NukeAsphaltTiles { get; set; } = false;
        public bool UpdateMovementMultiplierRunTime { get; set; } = false;
        public bool HaltBitumenProduction { get; set; } = false;
    }
}
