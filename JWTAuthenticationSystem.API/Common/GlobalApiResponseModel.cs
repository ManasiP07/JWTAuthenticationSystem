using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.Common
{
    public class GlobalApiResponseModel
    {
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; }

        [JsonProperty(PropertyName = "isStatusCodeSuccessful")]
        public bool IsStatusCodeSuccessful { get; set; }

        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; } = string.Empty;
    }
}