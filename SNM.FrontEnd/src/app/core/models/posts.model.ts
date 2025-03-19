import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { IPosts } from "../types/posts.types";
import { AuditableEntityBase } from "./shared/auditableEntityBase";

export class Posts extends AuditableEntityBase implements Required<IPosts> {
  caption: string;
  description: string | null;
  publicationDate: Date | null;
  publicationStatus: PublicationStatusEnum | null;
  socialChannels: {
    id?: string | null;
    displayName?: string | null;
    photo?: string | null;
    brandId?: string | null;
    channelTypeId?: string | null;
  }[]

  constructor(post: IPosts) {
    super();
    //super(post);
    this.caption = post.caption;
    this.description = post.description;
    this.publicationDate = post.publicationDate;
    this.publicationStatus = post.publicationStatus;
    this.socialChannels = [];

    // if (post.socialChannels) {
    //   this.socialChannels = post.socialChannels.map((channel) => {
    //     if (!(channel instanceof SocialChannel)) {
    //       return new SocialChannel(channel); 
    //     }
    //     return channel;
    //   });
    // }
  }
}