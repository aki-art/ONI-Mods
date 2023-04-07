using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace GravitasBigStorage
{
    public class Mod : UserMod2
    {
        public static Config Settings { get; private set; }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Log.PrintVersion();

            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(Config));
            Settings = POptions.ReadSettings<Config>() ?? new Config();
        }
    }
}
