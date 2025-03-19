import { IBrand, IFacebookChannelDto, ILinkedInChannel, IMember } from "../types/Brand.types";
import { ISocialChannel } from "../types/socialChannel.types";
import { LinkedInInsight } from "./linkedin-insight";
import { SocialChannel } from "./socialChannel.model";

export class Brand implements Required<IBrand> {
  id: string | null;
  displayName: string;
  description: string | null;
  photo: string | null;
  coverPhoto: string | null;
  socialChannels: SocialChannel[];
  
  // icon: string | null;
  // link: string | null;
  // useRouter: boolean | null;
  // lastActivity: string | null;
  // brandMembers: IMember[];

  constructor(brand: IBrand) {
    this.id = brand.id || null;
    this.displayName = brand.displayName;
    this.description = brand.description || null;
    this.photo = brand.photo || null;
    this.socialChannels = [];
    //this.link = "brand/channel";
    // this.lastActivity = brand.lastActivity || null;
    // this.brandMembers = [];

    if (brand.socialChannels) {
      this.socialChannels = brand.socialChannels.map((channel) => {
        if (!(channel instanceof SocialChannel)) {
          return new SocialChannel(channel);
        }
        return channel;
      });
    }

    // if (brand.brandMembers) {
    //   this.brandMembers = brand.brandMembers.map((member) => {
    //     if (!(member instanceof Member)) {
    //       return new Member(member);
    //     }
    //     return member;
    //   });
    // }
  }
}

export class FacebookChannelDto extends SocialChannel implements Required<IFacebookChannelDto> {
  channelId: string;
  channelAPI: string;
  userAccessToken: string;
  channelAccessToken: string;
  pageEdgeFeed: string;
  pageEdgePhotos: string;
  postToPageURL: string;
  postToPagePhotosURL: string;
  photo: string;
  brandId: string;

  constructor(facebookpage: IFacebookChannelDto) {
    super(facebookpage);
    this.channelId = facebookpage.channelId;
    this.channelAPI = facebookpage.channelAPI;
    this.userAccessToken = facebookpage.userAccessToken;
    this.channelAccessToken = facebookpage.channelAccessToken;
    this.pageEdgeFeed = facebookpage.pageEdgeFeed;
    this.pageEdgePhotos = facebookpage.pageEdgePhotos;
    this.postToPageURL = facebookpage.postToPageURL;
    this.postToPagePhotosURL = facebookpage.postToPagePhotosURL;
    this.photo = facebookpage.photo;
    this.brandId = facebookpage.brandId;
  }
}

export class LinkedInChannelDto extends SocialChannel implements Required<ILinkedInChannel> {
  companyName: string;
  companyId: string;
  companyUrn: string;
  coverPhoto: string;
  brandId: string;
  insight: LinkedInInsight;

  constructor(linkedinpage: ILinkedInChannel) {
    super(linkedinpage);
    this.companyName = linkedinpage.companyName;
    this.companyId = linkedinpage.companyId;
    this.companyUrn = linkedinpage.companyUrn;
    this.coverPhoto = linkedinpage.coverPhoto;
    this.brandId = linkedinpage.brandId;
  }
}

export class Member implements Required<IMember> {
  id: string | null;
  name: string;
  avatar: string | null;

  constructor(member: IMember) {
    this.id = member.id || null;
    this.name = member.name;
    this.avatar = member.avatar || null;
  }
}
