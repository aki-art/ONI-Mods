using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace SpookyPumpkinSO.Settings
{
    [ModInfo("Spooky Pumpkin", "assets/magicalpuftgirl3.png")]
    [JsonObject(MemberSerialization.OptIn)]
    public class SPSettings
    {
        [Option("Use rot for fertilizer", "If true, pumpkin plants will use Rot along Dirt for fertilization.")]
        [JsonProperty]
        public bool UseRot { get; set; }

        [Option("Suspicious Pip emits Light", "If true, the Suspicious Pip will emit some light.")]
        [JsonProperty]
        public bool GhostPipLight { get; set; }

        public SPSettings()
        {
            UseRot = true;
            GhostPipLight = true;
        }
    }
}
