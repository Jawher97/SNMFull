

using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Domain.Common;

namespace SNM.LinkedIn.Domain.Entities
{

    public class LinkedInComment: EntityBase<Guid>
    {
        
        public string CommentId { get; set; }
        public string ActivityUrn { get; set; }
        public string ActorUrn { get; set; }
        public string ActorUserName { get; set; }
        public string Actorprofilelink { get; set; }
        public string Actorheadline { get; set; }
        //public string ActorProfilePicture { get; set; }
        public string Comment { get; set; }
        public long Created_at { get; set; }
        public long Updated_at { get;set; }

        public string parentId { get; set; }

       // public List<string> LikedBy { get; set; }
        public string PostId { get; set; }
        public LinkedInInsight insight { get; set; }
        public List<LinkedInComment> SubCommentsList  { get; set; }
        

        public LinkedInComment()
        {
            insight = new LinkedInInsight();
        }

    }





}
