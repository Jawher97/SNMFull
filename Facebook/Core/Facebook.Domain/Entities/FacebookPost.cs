﻿using SNS.Facebook.Domain.Common;
using SNS.Facebook.Domain.Enumeration;


namespace SNS.Facebook.Domain.Entities
{
    public class FacebookPost: AuditableEntityBase<Guid>
    {
        public Guid PostId { get; set; }
        public Guid FacebookChannelId { get; set; }
        public PublicationStatusEnum PublicationStatus { get; set; }
        public DateTime? PublicationDate { get; set; }

        /*the proproties of Facbook Post in the social network */

        public string FacebookPostNetwokId { get; set; }
        public DateTime CreatedTime { get; set; }//The time the post was initially published.
        public FacebookPostFormatting Formatting { get; set; } //enum {MARKDOWN|PLAINTEXT}
        //public profile From { get; set; } //Information about the person or profile that posted the message.
        public string Icon { get; set; } //A link to an icon representing the type of this post.
        public string Link { get; set; } //The link attached to this post.
        public string Message { get; set; } //The status message in the post.
        public string Name { get; set; } //The name of the link, if attached to the post.
        public string ObjectId { get; set; } //The ID of any uploaded photo or video attached to the post.
        public string PermalinkUrl { get; set; } //URL to the permalink page of the post.
        public string Picture { get; set; } //The picture scraped from any link included with the post.
      //  public Place Place { get; set; } //Any location information attached to the post.
        //public object[] Properties { get; set; } //A list of properties for any attached video, for example, the length of the video.
        public FacebookPostStatusType StatusType { get; set; } //enum {mobile_status_update, created_note, added_photos, added_video, shared_story, created_group, created_event, wall_post, app_created_story, published_story, tagged_in_photo}
        public string Story { get; set; } //Text from stories not intentionally generated by users.
        // public Profile[] To { get; set; } //Text from stories not intentionally generated by users.
        public FacebookPostType Type { get; set; } //Tenum{link, status, photo, video}
        public DateTime UpdatedTime { get; set; } //The time when the post was created, last edited or the time of the last comment that was left on the post.
                                                  // public JSON WithTags { get; set; } //Profiles tagged as being 'with' the publisher of the post.JSON object with a data field that contains a list of Profile objects.
    
        /**********************************/


        public virtual FacebookChannel FacebookChannel { get; set; }
        public virtual Post Post { get; set; }

    }
}
