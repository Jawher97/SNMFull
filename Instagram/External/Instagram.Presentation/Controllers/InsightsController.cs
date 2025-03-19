using Microsoft.AspNetCore.Mvc;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Net.Http;
using Newtonsoft.Json;

namespace SNM.Instagram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsightsController : ControllerBase
    {
        private readonly IInsightRepository _insightRepository;


        public InsightsController(IInsightRepository insightRepository)
        {
            _insightRepository = insightRepository;
        }

        [HttpGet("Metrics")]
        public async Task<ActionResult> GetInsightsAsync(string period, string since, string until, string access_token)
        {
            string metrics = "comments,shares,follows,likes,profile_visits";

            string url = $"https://graph.facebook.com/v16.0/17988165941301486/insights?metric={metrics}&access_token={access_token}";

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.GetAsync(url);
                //var httpContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);

                    var comments = jsonObject["data"][0]["values"][0]["value"];
                    var shares = jsonObject["data"][1]["values"][0]["value"];
                    var follows = jsonObject["data"][1]["values"][0]["value"];
                    var likes = jsonObject["data"][1]["values"][0]["value"];
                    var profile_visits = jsonObject["data"][1]["values"][0]["value"];

                    Insight insight = new Insight
                    {
                        Comments = comments,
                        Shares = shares,
                        follows = follows,
                        likes= likes,
                        profile_visits=profile_visits
                    };

                    return Ok(insight);
                }

                return BadRequest();
            }
        }

    }
}