using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.DTOs
{
    public class RefreshTokenResponseDTO
    {
        [JsonProperty(PropertyName = "accessToken", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "refreshTokenExpiryDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}