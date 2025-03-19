using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Domain.Twitter
{
    public class TwitterLikesResponse
    {
        [JsonProperty("data")]
        public List<TwitterUser> Data { get; set; }

        [JsonProperty("meta")]
        public TwitterMetadata Metadata { get; set; }
    }

    public class TwitterUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    /*nombre total de résultats
     *le jeton suivant utilisé pour paginer les résultats.
     */
    public class TwitterMetadata
    {
        [JsonProperty("result_count")]
        public int ResultCount { get; set; }

        [JsonProperty("next_token")]
        public string NextToken { get; set; }
    }

}
