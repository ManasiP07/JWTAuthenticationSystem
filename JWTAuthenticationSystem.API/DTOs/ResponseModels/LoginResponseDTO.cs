using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.DTOs.Responses
{
    public class LoginResponseDTO
    {
        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid UId { get; set; }

        [JsonProperty(PropertyName = "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "refreshTokenExpiryDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}