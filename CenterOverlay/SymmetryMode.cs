using FUtility.FUI;
using Harmony;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace CenterOverlay
{
    public class SymmetryMode : OverlayModes.Mode
    {
        public override HashedString ViewMode() => ID;
        public static readonly HashedString ID = "MirrorSide";
        public override string GetSoundName() => "Decor";
    }
}
