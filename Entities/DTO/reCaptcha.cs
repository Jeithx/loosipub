using Newtonsoft.Json;

namespace Core.Entities.DTO
{
    public class reCaptcha
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
