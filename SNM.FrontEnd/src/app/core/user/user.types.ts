import { LinkedinProfileData } from "../models/linkedin-profile-data";
import { TwitterProfileData } from "../models/twitter-profile-data";

export interface User
{
    id: string;
    fullName: string;
    title: string;
    email: string;
    password: string;
    company: string;
    about: string;
    phone: string;
    rememberMe: boolean;
    token: string;
    country: string | null;
    avatar?: string | null;
    status?: string | null;

    linkedinProfile: LinkedinProfileData
    //twitterProfile: TwitterProfileData
}
