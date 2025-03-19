using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SNM.Instagram.Domain;
using SNM.Instagram.Domain.Interfaces;

namespace SNM.Instagram.Infrastructure.Repositories
{
    public class MediaDataRepository : IMediaDataRepository
    {
        private readonly ILogger<MediaDataRepository> _logger;
        private readonly HttpClient _httpClient;

        public MediaDataRepository(ILogger<MediaDataRepository> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MediaData>> GetMediaDataAsync(string accessToken, string igUserId)
        {
            var endpoint = $"https://graph.instagram.com/{igUserId}/media?access_token={accessToken}";

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var mediaData = JsonConvert.DeserializeObject<IEnumerable<MediaData>>(responseString);
                return mediaData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving media data from Instagram API");
                return null;
            }
        }
    }
}