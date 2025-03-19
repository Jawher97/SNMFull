using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;
using JobTimers.models;
using JobTimers.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace JobTimers.Services
{



    public class Services : IServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _backgroundJob;
        //private readonly ITenantService _tenantService;
        private readonly ILogger<Services> _logger;
        private readonly IConfiguration _configuration;

        private readonly TenantSettings _tenantSettings;
      
        public Services(IConfiguration configuration,IOptions<TenantSettings> tenantSettings,ApplicationDbContext context,  ILogger<Services> logger, IBackgroundJobClient backgroundJob)
        {
            _configuration = configuration;
            _context = context;
            _backgroundJob = backgroundJob;
            _logger = logger;
            //_tenantService = tenantService;
            _tenantSettings = tenantSettings.Value;
     

        }

      
        // get posts Linkedin,Instagram,Facebook,twitter
        public async Task SchedulePosts(string TenantTid)
        {
           

                    DateTime now = DateTime.Now;
                    DateTime endTime = now.AddMinutes(30);          
                    var facebookPosts = await GetAllFacebookposts(TenantTid);
                    var linkedInPosts = await GetAllLinkedInposts(TenantTid);
                    var twitterPosts = await GetAllTwitterposts(TenantTid);
                    var instagramPosts = await GetAllInstagramposts(TenantTid);
                    if (facebookPosts.Count > 0)
                    {
                        SchedulePostJobs(facebookPosts, now);
                    }
                    if (linkedInPosts.Count > 0)
                    {
                        SchedulePostJobs(linkedInPosts, now);
                    }
                    if (twitterPosts.Count > 0)
                    {
                        SchedulePostJobs(twitterPosts, now);
                    }
                    if (instagramPosts.Count > 0)
                    {
                        SchedulePostJobs(instagramPosts, now);
                    }
          

        }
        // get posts from DB
        private async Task<List<FacebookPostDto>> GetAllFacebookposts(string jobId)
        {
            List<FacebookPostDto> posts = new List<FacebookPostDto>();
         
                    var httpClient = new HttpClient();
                
                    // Set the tenant header.
                    httpClient.DefaultRequestHeaders.Add("tenant", jobId);
                    httpClient.BaseAddress = new Uri(_configuration["JobTimers:BaseUri"]);
                    // Send a GET request with the header.
                    var response = await httpClient.GetAsync(_configuration["FacebookAPi:GetAPI"]);
                    var strResult = response.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<List<FacebookPostDto>>(strResult);
             
            return posts;

        }
        private async Task<List<LinkedInPostDto>> GetAllLinkedInposts(string jobId)
        {
            List<LinkedInPostDto> posts = new List<LinkedInPostDto>();

            var httpClient = new HttpClient();

            // Set the tenant header.
            httpClient.DefaultRequestHeaders.Add("tenant", jobId);
            httpClient.BaseAddress = new Uri(_configuration["JobTimers:BaseUri"]);
            // Send a GET request with the header.
            var response = await httpClient.GetAsync(_configuration["LinkedInAPI:GetAPI"]);
            var strResult = response.Content.ReadAsStringAsync().Result;
            posts = JsonConvert.DeserializeObject<List<LinkedInPostDto>>(strResult);

            return posts;

        }
        private async Task<List<TwitterPostDto>> GetAllTwitterposts(string jobId)
        {
                    List<TwitterPostDto> posts = new List<TwitterPostDto>();

                    var httpClient = new HttpClient();

                    // Set the tenant header.
                    httpClient.DefaultRequestHeaders.Add("tenant", jobId);
                    httpClient.BaseAddress = new Uri(_configuration["JobTimers:BaseUri"]);
                    // Send a GET request with the header.
                    var response = await httpClient.GetAsync(_configuration["TwitterAPI:GetAPI"]);
                    var strResult = response.Content.ReadAsStringAsync().Result;
                    posts = JsonConvert.DeserializeObject<List<TwitterPostDto>>(strResult);

                    return posts;

        }
        private async Task<List<InstagramPostDto>> GetAllInstagramposts(string jobId)
        {
            List<InstagramPostDto> posts = new List<InstagramPostDto>();

            var httpClient = new HttpClient();

            // Set the tenant header.
            httpClient.DefaultRequestHeaders.Add("tenant", jobId);
            httpClient.BaseAddress = new Uri(_configuration["JobTimers:BaseUri"]);
            // Send a GET request with the header.
            var response = await httpClient.GetAsync(_configuration["InstagramAPI:GetAPI"]);
            var strResult = response.Content.ReadAsStringAsync().Result;
            posts = JsonConvert.DeserializeObject<List<InstagramPostDto>>(strResult);

            return posts;

        }
    
        // do schedule job
        private void SchedulePostJobs<T>(List<T> posts, DateTime now) where T : EntityBase<Guid>
        {
            if (posts.Count > 0)
            {
                foreach (var post in posts)
                {
                    DateTime publishtime = post.Publishtime;
                    TimeSpan timeDifference = publishtime - now;
                    int secondsToSubtract = 50;
                    TimeSpan newTimeSpan = timeDifference - TimeSpan.FromSeconds(secondsToSubtract);

                    // Schedule the job to run when publishtime is reached
                   BackgroundJob.Schedule(() => PublishSchedulePosts(post), newTimeSpan);
               
                }
            }
        }

        //publish to linkedin,facebook,twitter,instagram
        public async Task PublishSchedulePosts<T>(T post) where T : EntityBase<Guid>
        {
            var client = new HttpClient();
            // Example of accessing properties specific to the post type
            if (post is LinkedInPostDto linkedinPost)
            {
                try
                {
                    
                    client.BaseAddress = new Uri(_configuration["LinkedInAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");           
                    HttpResponseMessage response = await client.PostAsync(_configuration["LinkedInAPI:APIPublish"], jsonContent);
                  
                    if (response.IsSuccessStatusCode)
                    {
                        var strResult = await response.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        // Handle error
                        _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"An error occurred: {ex.Message}");
                }

            }
            if (post is FacebookPostDto facebookPost)
            {
                try
                {

                    
                    client.BaseAddress = new Uri(_configuration["FacebookAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                    //// Make the POST request to the Ocelot gateway endpoint.
                    HttpResponseMessage response = await client.PostAsync(_configuration["FacebookAPI:APIPublish"], jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var strResult = await response.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        // Handle error
              
                        _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.ToString());
                }

            }
           
            if (post is InstagramPostDto instagramPost)
            {
                try
                {
                    
                    client.BaseAddress = new Uri(_configuration["InstagramAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                    //// Make the POST request to the Ocelot gateway endpoint.
                    HttpResponseMessage response = await client.PostAsync(_configuration["InstagramAPI:APIPublish"], jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var strResult = await response.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        // Handle error
                        _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.ToString());
                }

            }
            if (post is TwitterPostDto twitterPost)
            {
                try
                {

                    client.BaseAddress = new Uri(_configuration["TwitterAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                    //// Make the POST request to the Ocelot gateway endpoint.
                    HttpResponseMessage response = await client.PostAsync(_configuration["TwitterAPI:APIPublish"], jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var strResult = await response.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        // Handle error
                        _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.ToString());
                }

            }
        }

        public async Task PublishSchedulePost<T>(T post, Guid channelId)
        {
                    if (post is FacebookPostDto facebookPost)
                    {

                var client = new HttpClient();
                        client.BaseAddress = new Uri(_configuration["FacebookAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                        //// Make the POST request to the Ocelot gateway endpoint.
                        HttpResponseMessage response = await client.PostAsync(_configuration["FacebookAPI:APIPublish"]+  channelId, jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var strResult = await response.Content.ReadAsStringAsync();

                        }
                        else
                        {
                            // Handle error

                            _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                        }

                    }
        
                if (post is LinkedInPostDto linkedinPost)
                    {

                        //LinkedIn
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(_configuration["LinkedInAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(_configuration["LinkedInAPI:APIPublish"]+channelId, jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var strResult = await response.Content.ReadAsStringAsync();

                        }
                        else
                        {
                            // Handle error
                            _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                        }

            }

        
               if (post is InstagramPostDto instagramPost)
            {


                        var client = new HttpClient();
                        client.BaseAddress = new Uri(_configuration["InstagramAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                        //// Make the POST request to the Ocelot gateway endpoint.
                        HttpResponseMessage response = await client.PostAsync(_configuration["InstagramAPI:APIPublish"] + channelId, jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var strResult = await response.Content.ReadAsStringAsync();

                        }
                        else
                        {
                            // Handle error
                            _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                        }
                      
                    }
             if (post is TwitterPostDto twitterPost)
{

                        var client = new HttpClient();
                        client.BaseAddress = new Uri(_configuration["TwitterAPI:BaseUri"]); // Replace with your actual Ocelot gateway URL                                                                                                        
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                        //// Make the POST request to the Ocelot gateway endpoint.
                        HttpResponseMessage response = await client.PostAsync(_configuration["TwitterAPI:APIPublish"]   +  channelId, jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var strResult = await response.Content.ReadAsStringAsync();

                        }
                        else
                        {
                            // Handle error
                            _logger.LogInformation($"HTTP request failed with status code {response.StatusCode}");
                        }

            }




        }
    }
}
