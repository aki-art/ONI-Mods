using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace GravitasBigStorage
{
    [ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
    [RestartRequired]
    public class Config
    {
        [Option("GravitasBigStorage.STRINGS.GRAVITASBIGSTORAGE.SETTINGS.CAPACITY.TITLE", "GravitasBigStorage.STRINGS.GRAVITASBIGSTORAGE.SETTINGS.CAPACITY.TOOLTIP")]
        [JsonProperty]
        public int Capacity { get; set; } = 250_000;
    }
}
