using System;
using Newtonsoft.Json;

namespace InfiniteFlightLiveClient.Types
{
    public class FlightDetailsEntry
    {
        [JsonProperty("altitude")]
        public float Altitude { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("speed")]
        public float Speed { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("track")]
        public float Track { get; set; }
        [JsonProperty("groundSpeed")]
        public double GroundSpeed { get; set; }
    }
}
