using Harmony;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldScrambler.Gen
{
    class WorldGenPatches
    {
        static bool loaded = false;
        [HarmonyPatch(typeof(SettingsCache), "LoadFiles")]
        public static class AddFakeAsteroid
        {
            public static void Prefix()
            {
                if (loaded) return;
                WorldGenerator.Initialize();
                loaded = true;
            }
        }
    }
}
