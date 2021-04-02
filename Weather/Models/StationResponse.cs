using Newtonsoft.Json;
using System.Collections.Generic;

namespace Weather.Models
{
    public class StationResponse
    {
        [JsonProperty("body")]
        public Dictionary<string, List<int>> Body { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("time_exec")]
        public decimal TimeExec { get; set; }

        [JsonProperty("time_server")]
        public long TimeServer { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
    public class Error
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
