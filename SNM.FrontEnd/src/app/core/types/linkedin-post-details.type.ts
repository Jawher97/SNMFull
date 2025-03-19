import { DateTime } from "luxon";

export interface LinkedinPostDetails {
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
export interface LinkedInInsight {

    totalComments: number;
    totalLikes: number;
    isLikedByAuthor: boolean;
    videoViews: number;
    shareCount: number;
    clickCount: number;
    impressionCount: number;
    engagement: number;

}
export interface LinkedinComment {

    commentId: string 
    activityUrn: string 
    actorUrn: string
    actorUserName: string 
    actorprofilelink: string
    actorheadline: string
    comment: string
    created_at: DateTime
    updated_at: DateTime
    parentId: null | string
    insight: LinkedInInsight
    subCommentsList:LinkedinComment[]

}