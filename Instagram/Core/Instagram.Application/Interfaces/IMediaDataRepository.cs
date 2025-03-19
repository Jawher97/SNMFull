
    using global::SNM.Instagram.Domain;
    using SNM.Instagram.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace SNM.Instagram.Domain.Interfaces
    {
        public interface IMediaDataRepository
        {
            Task<IEnumerable<MediaData>> GetMediaDataAsync(string accessToken, string igUserId);
        }
    }

