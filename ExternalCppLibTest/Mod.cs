using HarmonyLib;
using KMod;

namespace ExternalCppLibTest
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            //BitmapGenerator.Test();

            var lib = new Library();
        }
    }
}
