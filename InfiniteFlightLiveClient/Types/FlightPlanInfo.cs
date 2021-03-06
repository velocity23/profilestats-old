using Newtonsoft.Json;
using System;

namespace InfiniteFlightLiveClient.Types
{
    public class FlightPlanInfo
    {
        [JsonProperty("flightPlanItems")]
        public FlightPlanItem[] Items { get; set; }
        [JsonProperty("flightPlanId")]
        public Guid Id { get; set; }
        [JsonProperty("flightId")]
        public Guid FlightId { get; set; }
        [JsonProperty("waypoints")]
        public string[] Waypoints { get; set; }
        [JsonProperty("lastUpdate")]
        public DateTimeOffset LastUpdate { get; set; }
    }
}
