using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace InfiniteFlightLiveClient.Types
{
    public class GradeInfo
    {
        [JsonProperty("gradeDetails")]
        public GradeConfiguration GradeDetails { get; set; }
        
        //[JsonProperty("reports")]
        //public ReportEntry[] Reports { get; set; }

        //[JsonProperty("violations")]
        //public ViolationEntry[] Violations { get; set; }
        
        [JsonProperty("totalXP")]
        public int TotalXp { get; set; }

        [JsonProperty("total12MonthsViolations")]
        public int Total12MonthViolations { get; set; }

        [JsonProperty("violationCountByLevel")]
        public Dictionary<string, int> ViolationsByLevel { get; set; }

        [JsonProperty("lastLevel1ViolationDate")]
        public DateTimeOffset LastLevel1ViolationDate { get; set; }

        [JsonProperty("lastReportViolationDate")]
        public DateTimeOffset LastReportDate { get; set; } 

        [JsonProperty("roles")]
        public int[] Roles { get; set; }
        
        [JsonProperty("atcOperations")]
        public int AtcOperations { get; set; }
        
        [JsonProperty("atcRank")]
        public AtcRank? AtcRank { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("virtualOrganization")]
        public string VirtualOrganization { get; set; }

        [JsonProperty("discourseUsername")]
        public string DiscourseUsername { get; set; }
    }
}
