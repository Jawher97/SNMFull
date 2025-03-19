using Org.BouncyCastle.Asn1.Mozilla;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Domain.Enumeration;

public class InstagramPostDto:ModelBaseDto
    
    {

                
                    public string? Caption { get; set; }
    
                    public PublicationStatusEnum? PublicationStatus { get; set; }
                    public DateTime? PublicationDate { get; set; }
                    public virtual InstagramChannelDto? InstagramChannelDto { get; set; }

                    
                    public Guid? InstagramChannelId { get; set; }
                  
                    public string? InstagramPostId { get; set; }
                    public Guid? PostId { get; set; }
                    public PostDto? PostDto { get; set; }

}




//text
//public string Image_Url { get; set; }
//public string Video_url { get; set; }
//     public InstagramPostFormatting Formatting { get; set; } //enum {MARKDOWN|PLAINTEXT}
//  public string Link { get; set; } //The link attached to this post.
//public string Message { get; set; } //The status message in the post.
//public string PermalinkUrl { get; set; } //URL to the permalink page of the post.
//  public string ObjectId { get; set; }
//   public string Picture { get; set; } //The picture scraped from any link included with the post.
// public InstagramPostType Type { get; set; } //Tenum{link, status, photo, video}