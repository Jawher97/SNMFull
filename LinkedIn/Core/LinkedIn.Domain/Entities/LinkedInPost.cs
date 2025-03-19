using SNM.LinkedIn.Domain.Common;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Domain.Enumeration;

public class LinkedInPost : EntityBase<Guid>
{

    public Guid? PostId { get; set; }
 
    //public string Photo { get; set; }
    //public string Video { get; set; }
    //public string MediaUrn { get; set; }
    //public string MediaUrl { get; set; }
    public string PostUrn { get; set; }

    public PublicationStatusEnum PublicationStatus { get; set; }
    public DateTime? PublicationDate { get; set; }
    public string Message { get; set; }

   // public LinkedInInsight insight { get; set; }
    public Guid LinkedInChannelId { get; set; }
    public virtual LinkedInChannel LinkedInChannel { get; set; }
    public virtual Post? Post { get; set; }


}