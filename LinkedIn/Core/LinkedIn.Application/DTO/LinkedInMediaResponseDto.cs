
using System.Text.Json.Serialization;

namespace SNM.LinkedIn.Application.DTO
{
    public class LinkedInMediaResponseDto
    {
        [JsonPropertyName("uploadUrlExpiresAt")]
        public long uploadUrlExpiresAt { get; set; }

        [JsonPropertyName("uploadUrl")]
        public string uploadUrl { get; set; }

        [JsonPropertyName("image")]
        public string imageUrn { get ; set; }
    }
}
