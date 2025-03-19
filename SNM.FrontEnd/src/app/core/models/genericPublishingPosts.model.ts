import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { IGenericPublishingPosts } from "../types/genericPublishingPosts.types";
import { AuditableEntityBase } from "./shared/auditableEntityBase";
import { IChannelType } from "../types/channelType.types";
import { IBrand } from "../types/Brand.types";
import { MediaTypeEnum } from "../enumerations/MediaTypeEnum";

export class GenericPublishingPosts extends AuditableEntityBase implements Required<IGenericPublishingPosts> {
    message: string | null;
    publicationStatus: PublicationStatusEnum | null;
    publicationDate: Date | null;
    socialChannels: {
        id?: string | null;
        brand: IBrand;
        displayName: string | null;
        photo: string | null;
        brandId: string | null;
        channelTypeId: string | null;     
        channelType: IChannelType;
    }[];
    scheduleTime: Date | null;     
    mediaData: {
      media_type: MediaTypeEnum
      media_url?: string
    }[];


  constructor(genericPost: IGenericPublishingPosts) {
    //super(genericPost);
    super();
    this.message= genericPost.message || null;
    //this.publicationStatus = genericPost.publicationStatus || null;
    if (genericPost.publicationStatus !== null && genericPost.publicationStatus !== undefined) {
      this.publicationStatus = genericPost.publicationStatus;
    } else {
      this.publicationStatus = null;
    }
    this.publicationDate = genericPost.publicationDate || null;

    this.scheduleTime = genericPost.scheduleTime || null;
    this.socialChannels = [];
    this.mediaData=[];
  }
}