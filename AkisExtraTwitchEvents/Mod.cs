using FUtility;
using HarmonyLib;
using KMod;

namespace Twitchery
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAll();

            FUtility.Utils.RegisterDevTool<AETE_DevTool>("Mods/Akis Extra Twitch Events");
        }
    }
}
