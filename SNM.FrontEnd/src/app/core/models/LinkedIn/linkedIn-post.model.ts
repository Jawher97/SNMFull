import { PublicationStatusEnum } from "app/core/enumerations/PublicationStatusEnum";
import { AuditableEntityBase } from "../shared/auditableEntityBase";

export class LinkedInPosts extends AuditableEntityBase implements Required<LinkedInPosts> {
    postId: string | null;
    linkedInChannelId: string | null;
    publicationStatus: PublicationStatusEnum | null;

    constructor(linkedInPost: LinkedInPosts) {
        super(linkedInPost);
        this.postId = linkedInPost.postId || null;
        this.linkedInChannelId = linkedInPost.linkedInChannelId || null;
        this.publicationStatus = linkedInPost.publicationStatus || null;
    }
}
 