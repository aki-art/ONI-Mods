using FUtility;
using FUtility.SaveData;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Settings
{
    public class ElementColors : IUserSetting
    {
        [JsonIgnore]
        public Dictionary<SimHashes, Color> LiquidColors { get; set; }

        private Dictionary<SimHashes, string> ColorOverrides { get; set; }

        public void ProcessColors(ElementColors colors)
        {
            Log.Debuglog("Processing colors");
            LiquidColors = new Dictionary<SimHashes, Color>();

            if(ColorOverrides is null)
            {
                return;
            }

            foreach (var entry in ColorOverrides)
            {
                var color = Util.ColorFromHex(entry.Value);

                if (color != null)
                {
                    LiquidColors.Add(entry.Key, color);
                    Log.Debuglog($"Added {entry.Key}, {color}");
                }
                else
                {
                    Log.Warning($"Found color override for {entry.Key}, but it was incorrectly formatted, skipping.");
                }
            }
        }
    }
}
