using HarmonyLib;
using KMod;

namespace DuctTape
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Setup();
        }

        public void Setup()
        {
            ModAssets.StatusItems.Register();
        }
    }
}
