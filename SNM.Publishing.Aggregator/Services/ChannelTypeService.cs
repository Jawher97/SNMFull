using Newtonsoft.Json;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Text;

namespace SNM.Publishing.Aggregator.Services
{
    public class ChannelTypeService : IChannelTypeService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public ChannelTypeService(HttpClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
    }
}
