using InfiniteFlightLiveClient.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteFlightLiveClient
{
    public static class Client
    {
        private static string _apiKey;
        private static readonly string _baseUrl = "https://api.infiniteflight.com/public/v2";
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Get All Active Public Infinite Flight Servers
        /// </summary>
        /// <returns>SessionInfo Objects of All Active Public Servers</returns>
        public static async Task<List<SessionInfo>> GetSessionsAsync()
        {
            var json = await _httpClient.GetStringAsync(_baseUrl + $"/sessions?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<List<SessionInfo>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get All Active Public Infinite Flight Servers
        /// </summary>
        /// <returns>SessionInfo Objects of All Active Public Servers</returns>
        public static List<SessionInfo> GetSessions()
        {
            var json = _httpClient.GetStringAsync(_baseUrl + $"/sessions?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<List<SessionInfo>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get All Flights for a Session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>The Current Flights for the given Session</returns>
        public static async Task<List<FlightEntry>> GetFlightsAsync(Guid sessionId)
        {
            var json = await _httpClient.GetStringAsync(_baseUrl + $"/flights/{sessionId}?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<List<FlightEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get All Flights for a Session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>The Current Flights for the given Session</returns>
        public static List<FlightEntry> GetFlights(Guid sessionId)
        {
            var json = _httpClient.GetStringAsync(_baseUrl + $"/flights/{sessionId}?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<List<FlightEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get All Active ATC Facilities for a Session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>The Active Facilities for the given Session</returns>
        public static async Task<List<AtcEntry>> GetAtcFacilitiesAsync(Guid sessionId)
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/atc/{sessionId}?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<List<AtcEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get All Active ATC Facilities for a Session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>The Active Facilities for the given Session</returns>
        public static List<AtcEntry> GetAtcFacilities(Guid sessionId)
        {
            var json = _httpClient.GetStringAsync(_baseUrl + $"/atc/{sessionId}?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<List<AtcEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get the Flight Plan for an active Flight
        /// </summary>
        /// <param name="flightId">Flight ID</param>
        /// <returns>Information on the Flight Plan of the Requested Flight</returns>
        public static async Task<FlightPlanInfo> GetFlightPlanAsync(Guid flightId)
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/flight/{flightId}/flightplan?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<FlightPlanInfo>>(json);
            if (data.ErrorCode != ResponseCode.Ok && data.ErrorCode != ResponseCode.FlightNotFound)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get the Flight Plan for an active Flight
        /// </summary>
        /// <param name="flightId">Flight ID</param>
        /// <returns>Information on the Flight Plan of the Requested Flight</returns>
        public static FlightPlanInfo GetFlightPlan(Guid flightId)
        {
            var json = _httpClient.GetStringAsync($"{_baseUrl}/flight/{flightId}/flightplan?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<FlightPlanInfo>>(json);
            if (data.ErrorCode != ResponseCode.Ok && data.ErrorCode != ResponseCode.FlightNotFound)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get a User's Stats and Grade Table
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The Stats and Grade Table Information for the given User</returns>
        public static async Task<GradeInfo> GetUserGradeAsync(Guid userId)
        {
            var json = await _httpClient.GetStringAsync(_baseUrl + $"/user/grade/{userId}?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<GradeInfo>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get a User's Stats and Grade Table
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The Stats and Grade Table Information for the given User</returns>
        public static GradeInfo GetUserGrade(Guid userId)
        {
            var json = _httpClient.GetStringAsync(_baseUrl + $"/user/grade/{userId}?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<GradeInfo>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get Stats for One or More Users
        /// </summary>
        /// <param name="userIds">User IDs</param>
        /// <returns>The Users' Stats</returns>
        public static async Task<List<UserStats>> GetUserStatsAsync(Guid[] userIds = null, string[] hashes = null, string[] ifcNames = null)
        {
            if (userIds == null && hashes == null && ifcNames == null)
            {
                throw new ArgumentNullException(message: "One of userIds, hashes, or ifcNames is null", null);
            }

            var contentObj = new UserStatsRequest();
            if (userIds != null) contentObj.UserIds = userIds;
            if (hashes != null) contentObj.Hashes = hashes;
            if (ifcNames != null) contentObj.IfcNames = ifcNames;

            var contentJson = JsonConvert.SerializeObject(contentObj);
            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var resJson = await (await _httpClient.PostAsync(_baseUrl + $"/user/stats?apikey={_apiKey}", content)).Content.ReadAsStringAsync();
            var resData = JsonConvert.DeserializeObject<ApiResponse<List<UserStats>>>(resJson);

            if (resData.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {resData.ErrorCode}");
            }

            return resData.Result;
        }

        /// <summary>
        /// Get Stats for One or More Users
        /// </summary>
        /// <param name="userIds">User IDs</param>
        /// <returns>The Users' Stats</returns>
        public static List<UserStats> GetUserStats(Guid[] userIds = null, string[] hashes = null, string[] ifcNames = null)
        {
            if (userIds == null && hashes == null && ifcNames == null)
            {
                throw new ArgumentNullException(message: "One of userIds, hashes, or ifcNames is null", null);
            }

            var contentObj = new UserStatsRequest();
            if (userIds != null) contentObj.UserIds = userIds;
            if (hashes != null) contentObj.Hashes = hashes;
            if (ifcNames != null) contentObj.IfcNames = ifcNames;

            var contentJson = JsonConvert.SerializeObject(contentObj);
            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var resJson = _httpClient.PostAsync(_baseUrl + $"/user/stats?apikey={_apiKey}", content).Result.Content.ReadAsStringAsync().Result;
            var resData = JsonConvert.DeserializeObject<ApiResponse<List<UserStats>>>(resJson);

            if (resData.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {resData.ErrorCode}");
            }

            return resData.Result;
        }

        /// <summary>
        /// Get a Flight's Location History
        /// </summary>
        /// <param name="flightId">Flight ID</param>
        /// <returns>The Flight's Location History</returns>
        public static async Task<List<FlightDetailsEntry>> GetFlightDetailsAsync(Guid flightId)
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/flight/{flightId}/route?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<List<FlightDetailsEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok && data.ErrorCode != ResponseCode.ExpiredToken)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get a Flight's Location History
        /// </summary>
        /// <param name="flightId">Flight ID</param>
        /// <returns>The Flight's Location History</returns>
        public static List<FlightDetailsEntry> GetFlightDetails(Guid flightId)
        {
            var json = _httpClient.GetStringAsync($"{_baseUrl}/flight/{flightId}/route?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<List<FlightDetailsEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get the ATIS for an Airport
        /// </summary>
        /// <param name="airportIcao">ICAO of the Airport to get ATIS for</param>
        /// <param name="sessionId">ID of the Session (Server) the ATIS is on</param>
        /// <returns>The airport ATIS a string, or null if it isn't available</returns>
        public static async Task<string> GetAtisAsync(string airportIcao, Guid sessionId)
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/airport/{airportIcao}/atis/{sessionId}?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<string>>(json);
            if (data.ErrorCode != ResponseCode.Ok && data.ErrorCode != ResponseCode.NoAtisAvailable)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get the ATIS for an Airport
        /// </summary>
        /// <param name="airportIcao">ICAO of the Airport to get ATIS for</param>
        /// <param name="sessionId">ID of the Session (Server) the ATIS is on</param>
        /// <returns>The airport ATIS a string, or null if it isn't available</returns>
        public static string GetAtis(string airportIcao, Guid sessionId)
        {
            var json = _httpClient.GetStringAsync($"{_baseUrl}/airport/{airportIcao}/atis/{sessionId}?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<string>>(json);
            if (data.ErrorCode != ResponseCode.Ok && data.ErrorCode != ResponseCode.NoAtisAvailable)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get Active Oceanic Tracks
        /// </summary>
        /// <returns>Active Oceanic Tracks on Infinite Flight Multiplayer</returns>
        public static async Task<List<TrackEntry>> GetTracksAsync()
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/tracks?apikey={_apiKey}");
            var data = JsonConvert.DeserializeObject<ApiResponse<List<TrackEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        /// <summary>
        /// Get Active Oceanic Tracks
        /// </summary>
        /// <returns>Active Oceanic Tracks on Infinite Flight Multiplayer</returns>
        public static List<TrackEntry> GetTracks()
        {
            var json = _httpClient.GetStringAsync($"{_baseUrl}/tracks?apikey={_apiKey}").Result;
            var data = JsonConvert.DeserializeObject<ApiResponse<List<TrackEntry>>>(json);
            if (data.ErrorCode != ResponseCode.Ok)
            {
                throw new Exception($"Invalid API Response Code. Expected Ok, received {data.ErrorCode}");
            }

            return data.Result;
        }

        public static void SetApiKey(string newkey)
        {
            _apiKey = newkey;
        }

        private class UserStatsRequest
        {
            [JsonProperty("userIds")]
            public Guid[] UserIds { get; set; }
            [JsonProperty("userHashes")]
            public string[] Hashes { get; set; }
            [JsonProperty("discourseNames")]
            public string[] IfcNames { get; set; }
        }
    }

    public static class Roles
    {
        public const int Staff = 1;
        public const int Moderators = 2;
        public const int Ifatc = 64;
    }
}
