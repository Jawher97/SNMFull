using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.DTO
{
   
    public class LinkedInUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("profilePicture")]
        public ProfilePicture ProfilePicture { get; set; }

        [JsonPropertyName("vanityName")]
        public string VanityName { get; set; }

        [JsonPropertyName("localizedFirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("localizedLastName")]
        public string LastName { get; set; }

        [JsonPropertyName("localizedHeadline")]
        public string Headline { get; set; }

        [JsonPropertyName("firstName")]
        public LocalizedInformation LocalizedFirstName { get; set; }

        [JsonPropertyName("lastName")]
        public LocalizedInformation LocalizedLastName { get; set; }

        [JsonPropertyName("headline")]
        public LocalizedInformation LocalizedHeadline { get; set; }
    }
    public class ProfilePicture
    {
        [JsonPropertyName("displayImage")]
        public string DisplayImage { get; set; }
    }
    public class LocalizedInformation
    {
        [JsonPropertyName("localized")]
        public Dictionary<string, string> Localized { get; set; }

        [JsonPropertyName("preferredLocale")]
        public PreferredLocale PreferredLocale { get; set; }
    }
    public class PreferredLocale
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
