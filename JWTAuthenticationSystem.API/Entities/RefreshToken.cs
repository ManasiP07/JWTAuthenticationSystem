using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.Entities
{
    public class RefreshToken
    {
        [JsonProperty(PropertyName = "refreshTokenId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "tokenHash", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenHash { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "createdDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "isRevoked", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsRevoked { get; set; }

        [JsonProperty(PropertyName = "revokedDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? RevokedDate { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}