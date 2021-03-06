using ProfileStats.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileStats.Api.Services
{
    public static class SvgBuilder
    {
        public static string Generate(string content, string bg = null, int? height = null, int? width = null)
        {
            var style = bg == null ? string.Empty : $"style=\"background-color: {bg}\"";
            var h = height == null ? string.Empty : $"height=\"{height}\"";
            var w = width == null ? string.Empty : $"width=\"{width}\"";
            return $"<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" baseProfile=\"full\" {w} {h} {style}>" +
                "<style>text { font-family: sans-serif; } .bold { font-weight: bold; }</style>" +
                $"{content}" +
                "</svg>";
        }

        public static string FromData(string status, UserStats stats, string username)
        {
            var content = $"<text x=\"0\" y=\"15\" class=\"bold\">Stats for {username}</text>" +
                $"<text x=\"0\" y=\"35\">Online Flights: {stats.OnlineFlights}</text>" +
                $"<text x=\"0\" y=\"55\">Grade: Grade {stats.Grade}</text>" +
                $"<text x=\"0\" y=\"75\">Total XP: {stats.Xp}</text>" +
                $"<text x=\"0\" y=\"95\">Landing Count: {stats.LandingCount}</text>" +
                $"<text x=\"0\" y=\"115\">Total Flight Time: {stats.FlightTime}</text>" +
                $"<text x=\"0\" y=\"135\">ATC Operations: {stats.AtcOperations}</text>" +
                $"<text x=\"0\" y=\"155\">ATC Rank: {stats.AtcRank}</text>" +
                $"<text x=\"0\" y=\"185\" class=\"bold\">Status: {status}</text>";
            return Generate(content, "white", 200);
        }
    }
}
