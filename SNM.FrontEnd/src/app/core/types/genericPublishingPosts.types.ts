import { MediaTypeEnum } from "../enumerations/MediaTypeEnum";
import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { IAuditableEntityBase } from "./shared/auditableEntityBase.types";

export interface IGenericPublishingPosts extends IAuditableEntityBase {
  message?: string | null;
  publicationStatus?: PublicationStatusEnum | null;
  publicationDate?: Date | null;
  socialChannels?: {
    id?: string | null;
    displayName?: string | null;
    brandId?: string | null;
    channelTypeId?: string | null;
  }[];
  scheduleTime?: Date  | null;     
  mediaData?: {
    media_type: MediaTypeEnum
    media_url?: string
  }[];


  // Faccebook
  // formatting: FacebookPostFormatting | null;
  // icon: string | null;
  // link: string | null; 
  // name: string | null;
  // objectId: string | null;
  // permalinkUrl: string | null;
  // picture: string | null;
  // statusType: FacebookPostStatusType | null;
  // story: string | null;
  // type: FacebookPostType | null;
  // LinkedIn


}