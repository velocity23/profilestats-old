using Newtonsoft.Json;

namespace InfiniteFlightLiveClient.Types
{
    public class Coordinate
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
