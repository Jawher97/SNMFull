
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterOAuthRepository
    {
            Task<string> GetOAuthTokenAsync();

            Task<TwitterProfileData> GetAccessTokenAsync(string oAuthToken, string oAuthVerifier);

    }
}
