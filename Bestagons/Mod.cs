global using FUtility;
using Bestagons.Content.Map;
using Bestagons.Content.Scripts;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.IO;

namespace Bestagons
{
    public class Mod : UserMod2
    {
        public static HashSet<string> loadedModIds;
        public static Components.Cmps<PurchasableHex> hexes = new();
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.enableDebugLogging = true;
            Log.PrintVersion(this);
            Utils.RegisterDevTool<BestagonsHexPainterDevtool>("Mods/Bestagons_Hex Painter");
            Utils.RegisterDevTool<BestagonsDevTools>("Mods/Bestagons_General");
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            loadedModIds = new ();
            foreach (var mod in mods)
            {
                if (mod.IsEnabledForActiveDlc())
                    loadedModIds.Add(mod.staticID);
            }
        }

        public static string TryRead(string file)
        {
            try
            {
                return File.ReadAllText(file);
            }
            catch
            {
                // TODO
                return null;
            }
        }
    }
}