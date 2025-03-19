import { LinkedInChannelDto } from "./brand.model";

export interface LinkedinProfileData {
    
    LinkedInUserId: string;
    linkedinUrn: string;
    coverPhoto: string;
    fullName: string;
    linkedinProfileLink: string;
    headline: string;

    access_token: string;
    expires_in: string;
    refresh_token: string;
    scope: string;

    channels: LinkedInChannelDto[];

    
}
