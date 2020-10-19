using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpookyPumpkin
{
    public class ModAssets
    {
        public static string ModPath;
        public const string PREFIX = "SP_";
        public const string spooked = "SP_Spooked";

        public static void Initialize(string path)
        {
            ModPath = path;
        }

    }
}
