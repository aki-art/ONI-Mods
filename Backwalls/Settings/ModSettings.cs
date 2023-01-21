using Newtonsoft.Json;
using PeterHan.PLib.Options;
using UnityEngine;

namespace Backwalls.Settings
{
    [JsonObject(MemberSerialization.OptIn)]
    [ConfigFile(SharedConfigLocation: true)]
    public class ModSettings
    {
        [Option("Backwalls.STRINGS.UI.MODSETTINGS.LAYER.TITLE", "Backwalls.STRINGS.UI.MODSETTINGS.LAYER.TOOLTIP")]
        public WallLayer Layer { get; set; } = WallLayer.Automatic;

        [Option("Backwalls.STRINGS.UI.MODSETTINGS.DEFAULTPATTERN.TITLE", "Backwalls.STRINGS.UI.MODSETTINGS.DEFAULTPATTERN.TOOLTIP")]
        public LocText DefaultPattern { get; set; } = new LocText
        {
            text = "Tile"
        };

        [Option("Backwalls.STRINGS.UI.MODSETTINGS.DEFAULTCOLOR.TITLE", "Backwalls.STRINGS.UI.MODSETTINGS.DEFAULTCOLOR.TOOLTIP")]
        public Color DefaultColor { get; set; } = new Color(0.47058824f, 0.47058824f, 0.47058824f);

        public enum WallLayer
        {
            Automatic,
            BehindPipes,
            HidePipes
        }
    }
}
