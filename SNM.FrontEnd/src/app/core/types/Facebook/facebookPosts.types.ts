import { FacebookPostFormatting } from "app/core/enumerations/facebookPostFormatting";
import { FacebookPostStatusType } from "app/core/enumerations/FacebookPostStatusType";
import { FacebookPostType } from "app/core/enumerations/facebookPostType";
import { PublicationStatusEnum } from "app/core/enumerations/PublicationStatusEnum";
import { IAuditableEntityBase } from "../shared/auditableEntityBase.types";

export interface IFacebookPosts extends IAuditableEntityBase {
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
}

