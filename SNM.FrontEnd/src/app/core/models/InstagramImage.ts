import { MediaType } from "../enumerations/MedyaType";
import { IInstagramImage } from "../types/instagramImage.types";

export class InstagramImage implements Required<IInstagramImage> {
    id: string | null;
    media_type :MediaType 
    instagramPostlId:string
    media_url:string
  
    constructor(instagramImage: IInstagramImage) {
      this.id = instagramImage.id || null;
      this.media_type=instagramImage.media_type;
      this.media_url=instagramImage.media_url;
      this.instagramPostlId=instagramImage.instagramPostlId;
  
     
    }
  }