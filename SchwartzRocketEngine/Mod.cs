using KMod;
using System;

namespace SchwartzRocketEngine
{
    public class Mod : UserMod2
    {
        public const string ID = "SchwartzRocketEngine";
        public static string Prefix(string v) => $"{ID}_{v}";
    }
}
