using Hangfire;
using Hangfire.Storage;
using JobTimers.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobTimers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkedInPostsController : Controller
    {
        private readonly ApplicationDbContext _context;     
        protected ResponseDto _ResponseDto;
        public LinkedInPostsController(ApplicationDbContext context)
        {
            _context = context;
            this._ResponseDto = new ResponseDto();


        }

        //[HttpGet]
        //public async Task<List<LinkedInPostDto>> Get()
        //{

        //    List<LinkedInPostDto> objList = await _context.LinkedInPost.ToListAsync();


        //    return objList;
        //}

        //[HttpPost]
        //public async Task<object> Post([FromBody] LinkedInPostDto post)
        //{
        //    try
        //    {
        //        _context.LinkedInPost.Add(post);
        //        await _context.SaveChangesAsync();
        //        _ResponseDto.Result = post;
        //        _ResponseDto.IsSuccess = true;

        //    }
        //    catch (Exception e)
        //    {
        //        _ResponseDto.IsSuccess = false;
        //        _ResponseDto.ErrorMessages = new List<string>() { e.ToString() };
        //    }

        //    return _ResponseDto;
        //}
        //[HttpGet("GetPosts")]
        //public async Task<List<LinkedInPost>> GetPosts()
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime endTime = now.AddMinutes(30);
        //    List<LinkedInPost> objList = await _context.LinkedInPost
        //      .Where(post => (post.Publishtime >= now && post.Publishtime <= endTime) && post.Status == "publish")
        //      .ToListAsync();

        //    return objList;
        //}

        //[HttpGet("getPosts")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Getpost()
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44383/"); // Remplacez par votre URL réelle de passerelle Ocelot

        //    var response = await client.GetAsync("apiLinkedIn/v1/LinkedInAPI/PublishToLinkedIn");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var strResult = await response.Content.ReadAsStringAsync();
        //        return Ok(strResult);
        //    }
        //    else
        //    {
        //        return BadRequest("La demande a échoué."); 
        //    }
        //}



    }
}
