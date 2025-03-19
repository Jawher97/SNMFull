using Newtonsoft.Json;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SNM.Publishing.Aggregator.Services
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _client;

        public BrandService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Response<PostDto>> CreatePost(PostDto post ,ICollection<ChannelDto> Channels)
        {
            Response<PostDto> result = new Response<PostDto>();
             List<string>  channelType =new List<string>();
                try
            {
                foreach (var Channel in Channels)
                {
                    channelType.Add(Channel.ChannelType.Name);
                }

                // Use string.Join to concatenate the elements of the list into a single string
                string channeltypestring = string.Join(",", channelType);
                string brandUrl = "/api/v1/Post/CreatePost?ChannelTypeString="+channeltypestring;

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(post);
                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                // Send the POST request
                HttpResponseMessage response = await _client.PostAsync(brandUrl, content);

                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {                   
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<PostDto>>(responseContent)?.Data;

                    // Use the deserialized PostDto
                    if (responseObject != null)
                    {
                        result.Succeeded = true;
                        result.Data = responseObject;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Errors.Append("Response does not contain valid data.");
                    }

                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Append(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Append(ex.Message);
            }

            return result;

        }

    }
}
