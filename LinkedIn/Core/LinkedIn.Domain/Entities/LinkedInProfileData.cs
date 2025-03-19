using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SNM.LinkedIn.Domain.Entities
{
    public class LinkedInProfileData : EntityBase<Guid>
    {
        //public string author_username { get; set; }
        //public string author_profilelink { get; set; }
        public string LinkedInUserId { get; set; }
        public string LinkedinUrn { get; set; }
        public string? CoverPhoto { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string LinkedinProfileLink { get; set; }
        public string Headline { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
        public string scope { get; set; }
       /// <summary>
     //   public LinkedInPost linkedInPost { get; set; }
       /// </summary>

       // public List<LinkedInChannel> Channels { get; set; }
    }

    public class MemberProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string LinkedinProfileLink { get; set; }
        public string Headline { get; set; }
        //public string ProfilePicture { get; set; }
    }



    public class FullNameClass
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}
