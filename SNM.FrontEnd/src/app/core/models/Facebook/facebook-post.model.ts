import { FacebookPostFormatting } from "app/core/enumerations/facebookPostFormatting";
import { FacebookPostStatusType } from "app/core/enumerations/FacebookPostStatusType";
import { FacebookPostType } from "app/core/enumerations/facebookPostType";
import { PublicationStatusEnum } from "app/core/enumerations/PublicationStatusEnum";
import { AuditableEntityBase } from "../shared/auditableEntityBase";
import { IFacebookPosts } from "app/core/types/Facebook/facebookPosts.types";


export class FacebookPosts extends AuditableEntityBase implements Required<IFacebookPosts> {
    postId: string | null;
    facebookChannelId: string | null;
    publicationStatus: PublicationStatusEnum | null;

    // Properties of Facebook Post in the social network
    facebookPostNetwokId: string | null;
    createdTime: Date | null;
    formatting: FacebookPostFormatting | null;
    icon: string | null;
    link: string | null;
    message: string | null;
    name: string | null;
    objectId: string | null;
    permalinkUrl: string | null;
    picture: string | null;
    statusType: FacebookPostStatusType | null;
    story: string | null;
    type: FacebookPostType | null;
    updatedTime: Date | null;


    constructor(facebookPost: IFacebookPosts) {
        super(facebookPost);
        this.postId = facebookPost.postId || null;
        this.facebookChannelId = facebookPost.facebookChannelId || null;
        this.publicationStatus = facebookPost.publicationStatus || null;

        // Properties of Facebook Post in the social network
        this.facebookPostNetwokId = facebookPost.facebookPostNetwokId || null;
        this.createdTime = facebookPost.createdTime || null;
        this.formatting = facebookPost.formatting || null;
        this.icon = facebookPost.icon || null;
        this.link = facebookPost.link || null;
        this.message = facebookPost.message || null;
        this.name = facebookPost.name || null;
        this.objectId = facebookPost.objectId || null;
        this.permalinkUrl = facebookPost.permalinkUrl || null;
        this.picture = facebookPost.picture || null;
        this.statusType = facebookPost.statusType || null;
        this.story = facebookPost.story || null;
        this.type = facebookPost.type || null;
        this.updatedTime = facebookPost.updatedTime || null;

    }
}
 // export class FacebookPost {
 //     message:string;
 //     picture:string;
//     created_time:string;
 //     icon:string;
 //     permalink_url:string;
//     status_type:string;
 //     updated_time:string;
//     id:string;
 //     from:any;
 // }