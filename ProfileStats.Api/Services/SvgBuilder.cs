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

        public static string FromData(string status, UserStats stats, string username, string bg, string text)
        {
            var content = $"<text x=\"5\" y=\"20\" class=\"bold\" fill=\"{text}\">Stats for {username}</text>" +
                $"<text x=\"5\" y=\"40\" fill=\"{text}\">Online Flights: {stats.OnlineFlights:n0}</text>" +
                $"<text x=\"5\" y=\"60\" fill=\"{text}\">Grade: Grade {stats.Grade:n0}</text>" +
                $"<text x=\"5\" y=\"80\" fill=\"{text}\">Total XP: {stats.Xp:n0}</text>" +
                $"<text x=\"5\" y=\"100\" fill=\"{text}\">Landing Count: {stats.LandingCount:n0}</text>" +
                $"<text x=\"5\" y=\"120\" fill=\"{text}\">Total Flight Time: {stats.FlightTime}</text>" +
                $"<text x=\"5\" y=\"140\" fill=\"{text}\">ATC Operations: {stats.AtcOperations:n0}</text>" +
                $"<text x=\"5\" y=\"160\" fill=\"{text}\">ATC Rank: {stats.AtcRank}</text>" +
                $"<text x=\"5\" y=\"190\" class=\"bold\" fill=\"{text}\">Status: {status}</text>";
            return Generate(content, bg, 200);
        }
    }
}
