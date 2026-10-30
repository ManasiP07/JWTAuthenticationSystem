using Newtonsoft.Json;
namespace JWTAuthenticationSystem.API.DTOs
{
    public class RefreshTokenRequestDTO
    {
        [JsonProperty(PropertyName = "refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; } = string.Empty;
    }
}