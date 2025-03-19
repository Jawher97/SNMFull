namespace SNS.Facebook.Application.Identity.Tokens
{
    public record RefreshTokenRequest(string Token, string RefreshToken);
}