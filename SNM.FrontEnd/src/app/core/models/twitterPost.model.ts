import { IInstagramPost } from "../types/instagram-post.types";
import { MediaType } from "../enumerations/MedyaType";
import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { InstagramChannel } from "./instagram-channel";
import { DateTime } from "luxon";
export class TwitterPost {
     Text:string
     TwitterImagesDto:[]
     PublicationStatus :PublicationStatusEnum
     ScheduleTime :DateTime
     TwitterChannelId :string 
     PostId :string
}