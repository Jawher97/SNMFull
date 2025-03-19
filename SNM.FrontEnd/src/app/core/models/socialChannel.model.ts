import { ActivationStatus } from "../enumerations/ActivationStatus";
import { IBrand } from "../types/Brand.types";
import { IChannelType } from "../types/channelType.types";
import { ISocialChannel } from "../types/socialChannel.types";

export class SocialChannel implements Required<ISocialChannel> {
  id: string | null;
  displayName: string | null;
  photo: string | null;
  brandId: string | null;
  channelTypeId: string | null;
  brand: IBrand;
  channelType: IChannelType;
   isActivate:ActivationStatus 
   link :string | null;
   socialChannelId :string | null;
  constructor(socialChannel: ISocialChannel) {
    this.id = socialChannel.id || null;
    this.displayName = socialChannel.displayName;
    this.photo = socialChannel.photo;
    this.brandId = socialChannel.brandId;
    this.channelTypeId = socialChannel.channelTypeId;
    this.brand = socialChannel.brand;
    this.channelType = socialChannel.channelType;
    this.link=socialChannel.link;
    this.socialChannelId=socialChannel.socialChannelId;
    this.isActivate=socialChannel.isActivate;
   
  }
}