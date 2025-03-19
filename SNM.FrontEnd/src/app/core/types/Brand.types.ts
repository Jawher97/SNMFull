import { ISocialChannel } from "./socialChannel.types";

export interface IBrand
{
    id?: string | null;
    displayName: string;
    description?: string | null;
    photo?: string | null;
    coverPhoto?: string | null;
    socialChannels?: ISocialChannel[];
    // icon?: string| null;
    // link?: string| null;
    // useRouter?: boolean| null;
    // lastActivity?: string | null;   
    // brandMembers?: IMember[];
}

export interface IMember
{
    id?: string | null;
    name: string;
    avatar?: string | null;
}

export interface IFacebookChannelDto extends ISocialChannel {
    id?: string | null;
    displayName: string;
    channelId: string;
    //channelType:    ChannelTypeEnum;
    channelAPI: string;
    userAccessToken: string;
    channelAccessToken: string;
    pageEdgeFeed: string;
    pageEdgePhotos: string;
    postToPageURL: string;
    postToPagePhotosURL: string;
    photo: string;
    brandId: string;
}

export interface ILinkedInChannel extends ISocialChannel {
    id?: string | null;
    companyName: string;
    companyId: string;
    companyUrn: string;
    coverPhoto: string;
    brandId: string;
  }