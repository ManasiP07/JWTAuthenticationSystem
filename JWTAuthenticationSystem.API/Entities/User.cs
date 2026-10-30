using Newtonsoft.Json;

namespace JWTAuthenticationSystem.API.Entities
{
    public class User
    {
        [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "passwordHash", NullValueHandling = NullValueHandling.Ignore)]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "isActive", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "createdDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "updatedDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UpdatedDate { get; set; }
    }
}