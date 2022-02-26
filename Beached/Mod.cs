using Beached.Patches;
using HarmonyLib;
using KMod;

namespace Beached
{
    public class Mod : UserMod2
    {
        public static string Path;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Path = path;
            ModAssets.ZoneTypes.beach = EnumPatch.RegisterZoneType("Beach");
        }
    }
}
