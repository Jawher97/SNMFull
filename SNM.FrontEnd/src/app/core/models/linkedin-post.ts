import { LinkedInInsight } from "./linkedin-insight";

export interface LinkedinPost {

    Id: string
    postId: string;
    author_urn: string; //person or organization
    author_username: string
    author_profilelink: string
    photo: string
    video: string
    message: string;
    createdAt: Date;
    mediaUrn: string;
    mediaUrl: string;
    insight: LinkedInInsight;




}
