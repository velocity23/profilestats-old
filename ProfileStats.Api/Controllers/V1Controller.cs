using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileStats.Api.Models;
using ProfileStats.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InfiniteFlightLiveClient;
using InfiniteFlightLiveClient.Types;

namespace ProfileStats.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class V1Controller : ControllerBase
    {
        private readonly RedisService _redis;

        public V1Controller(RedisService redis)
        {
            _redis = redis;
        }

        [HttpGet("{ifc}.svg")]
        public async Task<IActionResult> Get([FromRoute, Required] string ifc, [FromQuery] string bg = "white", string text = "black")
        {
            _redis.UseDatabse(RedisDatabase.NameIds);
            var uid = await _redis.GetString(ifc.ToLower());
            if (uid == default)
            {
                try
                {
                    var lookup = await Client.GetUserStatsAsync(ifcNames: new[] { ifc });
                    if (lookup.Count < 1)
                    {
                        throw new Exception("User not found");
                    }

                    uid = lookup.First().UserId.ToString();
                    _redis.SetString(ifc.ToLower(), uid);
                }
                catch
                {
                    return Content(SvgBuilder.Generate("<text x=\"0\" y=\"15\" fill =\"red\" class=\"bold\">Failed to Load Stats. Ensure you've enabled username display in-flight.</text>"), "image/svg+xml");
                }
            }

            _redis.UseDatabse(RedisDatabase.Responses);
            var cached = await _redis.GetString($"{uid}_{bg}_{text}");
            if (cached != default) return Content(cached, "image/svg+xml; charset=utf-8");

            _redis.UseDatabse(RedisDatabase.Misc);
            var sessions = await _redis.GetObject<List<SessionInfo>>("sessions");
            if (sessions == default)
            {
                sessions = await Client.GetSessionsAsync();
                _redis.SetObject("sessions", sessions);
            }

            var flights = await _redis.GetObject<List<FlightEntry>>("flights");
            if (flights == default)
            {
                var flightsSessions = new List<List<FlightEntry>>();
                foreach (var s in sessions)
                {
                    flightsSessions.Add(await Client.GetFlightsAsync(s.Id));
                }
                flights = flightsSessions.SelectMany(x => x).ToList();
                _redis.SetObject("flights", flights, TimeSpan.FromMinutes(10));
            }

            var atc = await _redis.GetObject<List<AtcEntry>>("atc");
            if (atc == default)
            {
                var atcSessions = new List<List<AtcEntry>>();
                foreach (var s in sessions)
                {
                    atcSessions.Add(await Client.GetAtcFacilitiesAsync(s.Id));
                }
                atc = atcSessions.SelectMany(x => x).ToList();
                _redis.SetObject("atc", atc, TimeSpan.FromMinutes(10));
            }

            var status = "Not Active";
            var flight = flights.FirstOrDefault(f => f.UserId.ToString() == uid);
            var stations = atc.GroupBy(a => a.UserId).FirstOrDefault(g => g.Any(a => a.UserId.ToString() == uid));
            if (flight != default)
            {
                try
                {
                    _redis.UseDatabse(RedisDatabase.FlightPlans);
                    var fpl = await _redis.GetObject<FlightPlanInfo>(flight.FlightId.ToString());
                    if (fpl == default)
                    {
                        fpl = await flight.GetFlightPlan();
                        _redis.SetObject(flight.FlightId.ToString(), fpl, TimeSpan.FromHours(24));
                    }
                    if (fpl.Waypoints.Length < 2)
                    {
                        throw new Exception("Not Enough Waypoints");
                    }

                    if (fpl.Waypoints.First().Length != 4 && fpl.Waypoints.Last().Length != 4)
                    {
                        throw new Exception("Invalid Departure/Arrival Waypoints");
                    }

                    if (fpl.Waypoints.First().Length != 4)
                    {
                        status = $"Flying to {fpl.Waypoints.Last()}";
                    } 
                    else if (fpl.Waypoints.Last().Length != 4)
                    {
                        status = $"Flying from {fpl.Waypoints.First()}";
                    }
                    else
                    {
                        status = $"Flying from {fpl.Waypoints.First()} to {fpl.Waypoints.Last()}";
                    }
                }
                catch
                {
                    status = "Flying";
                }
            }
            else if (stations != default && stations.Any())
            {
                if (stations.Any(s => s.AirportName == null))
                {
                    status = $"Controlling {string.Join(", ", stations.Select(s => s.Type.ToString()).ToArray())}";
                }
                else
                {
                    status = $"Controlling {stations.First().AirportName} {string.Join(", ", stations.Select(s => s.Type.ToString()).ToArray())}";
                }
            }

            _redis.UseDatabse(RedisDatabase.Stats);
            var stats = await _redis.GetObject<Models.UserStats>(uid);
            if (stats == default)
            {
                stats = (await Client.GetUserStatsAsync(new[] { Guid.Parse(uid) })).Select(x => new Models.UserStats {
                    AtcOperations = x.AtcOperations,
                    AtcRank = x.AtcRank.ToString(),
                    FlightTime = $"{Math.Floor(x.FlightTime / 60)}:{x.FlightTime % 60}",
                    Grade = x.Grade,
                    LandingCount = x.LandingCount,
                    OnlineFlights = x.OnlineFlights,
                    Xp = x.Xp,
                }).FirstOrDefault();
                _redis.SetObject(uid, stats, TimeSpan.FromHours(24));
            }

            _redis.UseDatabse(RedisDatabase.Responses);
            var data = SvgBuilder.FromData(status, stats, ifc, bg, text);
            _redis.SetString($"{uid}_{bg}_{text}", data, TimeSpan.FromMinutes(10));

            return Content(data, "image/svg+xml; charset=utf-8");
        }
    }
}
