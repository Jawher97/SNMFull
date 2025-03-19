using JobTimers.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTimers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : Controller
    {
        private readonly ApplicationDbContext _context;
        protected ResponseDto _ResponseDto;
        public TwitterController(ApplicationDbContext context)
        {
            _context = context;
            this._ResponseDto = new ResponseDto();


        }

        //    [HttpGet("GetPosts")]
        //    public async Task<List<TwitterPost>> GetPosts()
        //    {
        //        DateTime now = DateTime.Now;
        //        DateTime endTime = now.AddMinutes(30);
        //        List<TwitterPost> objList = await _context.TwitterPost
        //          .Where(post => (post.Publishtime >= now && post.Publishtime <= endTime) && post.Status == "publish")
        //          .ToListAsync();

        //        return objList;
        //    }
        //    [HttpPost]
        //    public async Task<object> Post([FromBody] TwitterPost post)
        //    {
        //        try
        //        {
        //            _context.TwitterPost.Add(post);
        //            await _context.SaveChangesAsync();
        //            _ResponseDto.Result = post;
        //            _ResponseDto.IsSuccess = true;

        //        }
        //        catch (Exception e)
        //        {
        //            _ResponseDto.IsSuccess = false;
        //            _ResponseDto.ErrorMessages = new List<string>() { e.ToString() };
        //        }

        //        return _ResponseDto;
        //    }
        //}
    }
}