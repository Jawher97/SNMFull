
using System.Collections.Generic;
using SNM.Instagram.Application.DTO;


namespace SNM.Instagram.Application.Features.Queries.InstagramPost
{

    public class GetInstagramPostViewModel
{
    public string SearchTerm { get; set; }
    public List<InstagramPostDto> Post { get; set; }
}
}