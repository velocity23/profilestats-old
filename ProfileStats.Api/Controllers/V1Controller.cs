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
        public async Task<IActionResult> Get([FromRoute, Required] string ifc)
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
            var cached = await _redis.GetString(uid);
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

            var status = "Not Flying";
            var flight = flights.FirstOrDefault(f => f.UserId.ToString() == uid);
            if (flight != default)
            {
                try
                {
                    var fpl = await flight.GetFlightPlan();
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
            var data = SvgBuilder.FromData(status, stats, ifc);
            _redis.SetString(uid, data, TimeSpan.FromMinutes(5));

            return Content(data, "image/svg+xml; charset=utf-8");
        }

        [HttpGet("test.svg")]
        public IActionResult Test()
        {
            var svg = SvgBuilder.Generate("<text x=\"0\" y=\"15\">Hello<tspan class=\"bold\">World!</tspan></text>", "white");
            return Content(svg, "image/svg+xml; charset=utf-8");
        }
    }
}
