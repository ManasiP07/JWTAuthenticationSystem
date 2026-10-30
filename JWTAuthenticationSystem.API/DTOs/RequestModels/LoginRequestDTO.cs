using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.DTOs.RequestModels
{
    public class LoginRequestDTO
    {
        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; } = string.Empty;

    }
}
