import { FacebookPostFormatting } from "../enumerations/facebookPostFormatting";
import { FacebookPostStatusType } from "../enumerations/FacebookPostStatusType";
import { FacebookPostType } from "../enumerations/facebookPostType";
import { FacebookChannel } from "./FacebookChannel";
import { Post } from "./post.model";

export interface FacebookChannelPost {
    facebookPostId: string;
    createdTime: string;
    formatting: FacebookPostFormatting;
    icon: string;
    link: string;
    message: string;
    name: string;
    objectId: string;
    permalinkUrl: string;
    picture: string;
    properties: any[];
    statusType: FacebookPostStatusType;
    story: string;
    type: FacebookPostType;
    updatedTime: string;
    facebookChannelId: string;
    facebookChannelDto: FacebookChannel;
    postId: string;
    post: Post;
}