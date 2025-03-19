namespace AuthAPI.DTO
{
    public class TokenApiDto
    {
        public Guid CurrentUserId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
