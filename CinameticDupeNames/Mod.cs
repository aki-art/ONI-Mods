using CinematicDupeNames.Content;
using FUtility;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;

namespace CinematicDupeNames
{
    public class Mod : UserMod2
    {

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion(this);

            PUtil.InitLibrary();
            CDNActions.Register();
        }
    }
}
