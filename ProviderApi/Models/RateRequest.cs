using System;
using Newtonsoft.Json;

namespace ProviderApi.Models
{
    public class RateRequest
    {
        [JsonProperty("groupId")]
        public Guid? GroupId { get; set; }
        
        [JsonProperty("beginTime")]
        public string BeginTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }
}