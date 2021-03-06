using System;
using Newtonsoft.Json;

namespace InfiniteFlightLiveClient.Types
{
    public class TrackEntry
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("path")]
        public string[] Path { get; set; }
        [JsonProperty("eastLevels")]
        public int[] EastLevels { get; set; }
        [JsonProperty("westLevels")]
        public int[] WestLevels { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("lastSeen")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
