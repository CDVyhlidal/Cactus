using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cactus.Models
{
    public class Entry
    {
        [JsonProperty("Label", Order = 1)]
        public string Label { get; set; }

        [JsonProperty("Version", Order = 2)]
        public string Version { get; set; }

        [JsonProperty("Flags", Order = 3)]
        public string Flags { get; set; }

        [JsonProperty("IsExpansion", Order = 4)]
        public bool IsExpansion { get; set; }

        [JsonProperty("WasLastRan", Order = 5)]
        public bool WasLastRan { get; set; }
    }
}
