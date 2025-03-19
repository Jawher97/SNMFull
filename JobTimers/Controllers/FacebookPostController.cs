using Hangfire;
using JobTimers.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JobTimers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacebookPostController : Controller
    {
        private readonly ApplicationDbContext _context;
        protected ResponseDto _ResponseDto;
        public FacebookPostController(ApplicationDbContext context)
        {
            _context = context;
            this._ResponseDto = new ResponseDto();


        }
        // [HttpGet]
        //public async Task<List<FacebookPostDto>> Get()
        //{

        //    List<FacebookPostDto> objList = await _context.facebookPost.ToListAsync();


        //    return objList;
        //}
        //[HttpGet("GetPosts")]
        //public async Task<List<FacebookPostDto>> GetPosts()
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime endTime = now.AddMinutes(30);
        //    List<FacebookPostDto> objList = await _context.facebookPost
        //      .Where(post => (post.Publishtime >= now && post.Publishtime <= endTime) && post.Status == "publish")
        //      .ToListAsync();

        //    return objList;
        //}
        //[HttpPost]
        //public  async Task<object> Post([FromBody] FacebookPostDto post)
        //{
        //    try
        //    {
        //        _context.facebookPost.Add(post);
        //         await _context.SaveChangesAsync();
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

    }
}