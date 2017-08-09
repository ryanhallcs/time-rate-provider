using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NodaTime;

namespace ProviderApi.Models
{
    public class RawRateGroup
    {
        [JsonProperty("days")]
        public string Days { get; set; }

        [JsonProperty("times")]
        public string Times { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }
    }
    
    public class RawRateGroups
    {
        [JsonProperty("rates")]
        public List<RawRateGroup> Rates { get; set; }
    }

    public class RateGroup
    {
        public Guid GroupId { get; set; }
        public Dictionary<IsoDayOfWeek, List<TimeRangeRate>> DayRateRanges { get; set; } = new Dictionary<IsoDayOfWeek, List<TimeRangeRate>>();
    }
}