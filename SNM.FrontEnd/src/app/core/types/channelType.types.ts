import { IBrand } from "./Brand.types";
import { ISocialChannel } from "./socialChannel.types";

export interface IChannelType
{
    id?: string | null;
    name: string;
    description?: string | null;
    icon?: string | null;  
    channels?: ISocialChannel[];
}
export interface IChannel
{
    id?: string | null;
    displayName?: string | null;
    photo?: string | null;
    brandId?: string | null;
    channelTypeId?: string | null;
    link :string | null;
    socialChannelId :string | null;
    brand?: IBrand  | null;
    channelType?: IChannelType  | null;
}