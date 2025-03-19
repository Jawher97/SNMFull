import { MediaType } from "../enumerations/MedyaType";

export interface IInstagramImage {
    id: string | null;
    media_type :MediaType
    instagramPostlId:string
    media_url:string
}