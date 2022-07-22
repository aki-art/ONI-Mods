using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backwalls
{
    public class Mod : UserMod2
    {
        public static BackwallRenderer renderer;
        public static bool isTrueTilesHere;
        public static List<BackwallVariant> variants = new List<BackwallVariant>();

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            isTrueTilesHere = mods.Any(mod => mod.staticID == "TrueTiles" && mod.IsEnabledForActiveDlc());

            if(isTrueTilesHere)
            {
                ModAssets.uiSprites = new Dictionary<string, Sprite>();
                BackwallRenderer.trueTiles = new Dictionary<HashedString, BackwallRenderer.TrueTileTexture>();
                Integration.TrueTilesPatches.Patch(harmony);
            }
        }
    }
}
