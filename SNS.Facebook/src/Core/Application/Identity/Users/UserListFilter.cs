namespace SNS.Facebook.Application.Identity.Users
{
    public class UserListFilter : PaginationFilter
    {
        public bool? IsActive { get; set; }
    }
}