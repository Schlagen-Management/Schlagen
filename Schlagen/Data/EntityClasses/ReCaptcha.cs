using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class ReCaptcha
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("challenge_ts")]
        public string ChallengeTS { get; set; }
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
        [JsonPropertyName("score")]
        public decimal Score { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; }
    }
}
