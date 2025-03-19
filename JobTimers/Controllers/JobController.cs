using Hangfire;
using Hangfire.Storage;
using JobTimers.models;
using JobTimers.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTimers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _backgroundJob;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IServices _service;

        public JobController(ApplicationDbContext context, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServices service)
        {
            _context = context;
            _backgroundJob = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _service = service;




        }

        [HttpGet("getlastJob")]
        public IActionResult GetLastJob()
        {

            var connection = JobStorage.Current.GetConnection();
            var lastRecurringJob = connection.GetRecurringJobs().LastOrDefault();

            if (lastRecurringJob != null)
            {
                var jobId = lastRecurringJob.Id;
                var jobTypeName = lastRecurringJob.Job.Type.Name;
                var jobMethodName = lastRecurringJob.Job.Method.Name;
                DateTime? lastExecution = lastRecurringJob.LastExecution;
                DateTime? lastExecutionLocal = null;
                if (lastExecution.HasValue)
                {
                    // Convert last execution time to local time if it has a value
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    lastExecutionLocal = TimeZoneInfo.ConvertTimeFromUtc(lastExecution.Value, localTimeZone);
                }

                var jobInfo = new
                {
                    jobId = jobId,
                    lastExecution = lastExecutionLocal,

                    jobTypeName = jobTypeName,
                    jobMethodName = jobMethodName

                };

                // Return the last recurring job as JSON
                return Json(jobInfo);
            }
            else
            {
                return Ok("No recurring jobs found.");
            }

        }
        [HttpPost("LinkedInSchedulePost")]
        public async  Task<Response<string>> LinkedInSchedulePost([FromBody] LinkedInPostDto post, [FromQuery] Guid channelId) 
        {
            Response<string> resultjob = new Response<string>();
            if (post!=null)
            {
                DateTime now = DateTime.Now;
                // DateTime publishtime = post.ScheduleTime;
                TimeSpan timeDifference = post.PublicationDate - now;
                int secondsToSubtract = 50;
                TimeSpan newTimeSpan = timeDifference - TimeSpan.FromSeconds(secondsToSubtract);

                // Schedule the job to run when publishtime is reached
                BackgroundJob.Schedule(() => _service.PublishSchedulePost(post, channelId), timeDifference);
                resultjob.Succeeded= true;
                
                return resultjob;
            }


            return resultjob;


        }
        [HttpPost("FacebookSchedulePost")]
        public void FacebookSchedulePost([FromBody] FacebookPostDto post, [FromQuery] Guid channelId) 
        {

            DateTime now = DateTime.Now;
            DateTime publishtime = post.PublicationDate;
            TimeSpan timeDifference = publishtime - now;
         

            // Schedule the job to run when publishtime is reached
            BackgroundJob.Schedule(() => _service.PublishSchedulePost(post, channelId), timeDifference);



        }
        [HttpPost("InstagramSchedulePost")]
        public void InstagramSchedulePost([FromBody] InstagramPostDto post, [FromQuery] Guid channelId) 
        {

            DateTime now = DateTime.Now;
            DateTime publishtime = post.PublicationDate;
            TimeSpan timeDifference = publishtime - now;
        

            // Schedule the job to run when publishtime is reached
            BackgroundJob.Schedule(() => _service.PublishSchedulePost(post, channelId), timeDifference);



        }
        [HttpPost("TwitterSchedulePost")]
        public void TwitterSchedulePost([FromBody] TwitterPostDto post, [FromQuery] Guid channelId) 
        {

            DateTime now = DateTime.Now;
            DateTime publishtime = post.PublicationDate;
            TimeSpan timeDifference = publishtime - now;
          

            // Schedule the job to run when publishtime is reached
            BackgroundJob.Schedule(() => _service.PublishSchedulePost(post, channelId), timeDifference);



        }
    }
}