using FUtility;
using HarmonyLib;
using KMod;

namespace ElementSpam
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
