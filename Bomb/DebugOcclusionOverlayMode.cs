using Harmony;
using System;
using System.Collections.Generic;

namespace Bomb
{
    class DebugOcclusionOverlayMode
    {
        public class BombMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "BombOverlay";
            public override string GetSoundName() => "Decor";
        }
    }
}