using SNM.LinkedIn.Domain.Common;

namespace SNM.LinkedIn.Domain.Entities
{
    public class LinkedInChannel : EntityBase<Guid>
    {
        public string Author_urn { get; set; }
        public string AccessToken { get; set; }        
        
       public virtual Channel SocialChannel { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


        public Guid ChannelId { get; set; }

       // public virtual Channel SocialChannel { get; set; }

        //public Guid BrandId { get; set; }

        //public LinkedInChannel()
        //{
        //    //insight = new LinkedInInsight();
        //}
    }
}
