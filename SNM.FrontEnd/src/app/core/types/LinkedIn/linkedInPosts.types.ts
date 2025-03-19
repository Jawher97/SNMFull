import { PublicationStatusEnum } from "app/core/enumerations/PublicationStatusEnum";
import { IAuditableEntityBase } from "../shared/auditableEntityBase.types";

export interface IlinkedInPosts extends IAuditableEntityBase {
    postId: string | null;
    linkedInChannelId: string | null;
    publicationStatus: PublicationStatusEnum | null;

  
}

