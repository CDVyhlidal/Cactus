using Newtonsoft.Json;

namespace Cactus.Models
{
    public class EntryModel
    {
        [JsonProperty("Label", Order = 1)]
        public string Label { get; set; }

        [JsonProperty("Version", Order = 2)]
        public string Version { get; set; }

        [JsonProperty("Path", Order = 3)]
        public string Path { get; set; }

        [JsonProperty("Flags", Order = 4)]
        public string Flags { get; set; }

        [JsonProperty("IsExpansion", Order = 5)]
        public bool IsExpansion { get; set; }

        [JsonProperty("WasLastRan", Order = 6)]
        public bool WasLastRan { get; set; }
    }
}
