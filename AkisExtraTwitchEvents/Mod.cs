using FUtility;
using HarmonyLib;
using KMod;
using Twitchery.Content.Scripts;

namespace Twitchery
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<MidasToucher> midasTouchers = new();
        public static Components.Cmps<GiantCrab> giantCrabs = new ();

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAll();

            FUtility.Utils.RegisterDevTool<AETE_DevTool>("Mods/Akis Extra Twitch Events");
        }
    }
}
