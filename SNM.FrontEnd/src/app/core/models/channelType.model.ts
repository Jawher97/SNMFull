import { IChannelType } from "../types/channelType.types";
import { ISocialChannel } from "../types/socialChannel.types";
import { SocialChannel } from "./socialChannel.model";

export class ChannelType implements Required<IChannelType> {
    id: string | null;
    name: string;
    description: string | null;
    icon: string | null;  
    // socialChannels: ISocialChannel[];
    channels: ISocialChannel[];

  constructor(channelType: IChannelType) {
    this.id = channelType.id || null;
    this.name = channelType.name;
    this.description = channelType.description || null;
    this.icon = channelType.icon || null;  
    // this.socialChannels = [];
    this.channels = [];
  
   
     if (channelType.channels) {
      this.channels = channelType.channels.map((channel) => {
        if (!(channel instanceof SocialChannel)) {
          return new SocialChannel(channel);
        }
        return channel;
      });
    }

  } 
}
