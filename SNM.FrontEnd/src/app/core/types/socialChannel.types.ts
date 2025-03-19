import { ActivationStatus } from "../enumerations/ActivationStatus";
import { IBrand } from "./Brand.types";
import { IChannelType } from "./channelType.types";

export interface ISocialChannel
{
    id?: string | null;
    displayName?: string | null;
    photo?: string | null;
    brandId?: string | null;
    channelTypeId?: string | null;
     link? :string | null;
     socialChannelId? :string | null;
    brand?: IBrand  | null;
    channelType?: IChannelType  | null;
    isActivate:ActivationStatus 
}
