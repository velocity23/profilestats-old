using Newtonsoft.Json;

namespace InfiniteFlightLiveClient.Types
{
    public class FlightPlanItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public PlanItemType Type { get; set; }
        [JsonProperty("children")]
        public FlightPlanItem[] Children { get; set; }
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("altitude")]
        public int Altitude { get; set; }
        [JsonProperty("location")]
        public Coordinate Location { get; set; }
    }

    public enum PlanItemType
    {
        Sid,
        Star,
        Approach,
        Track,
        Unknown,
    }
}
