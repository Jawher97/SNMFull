namespace SNS.Facebook.Application.Identity.Tokens
{
    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}