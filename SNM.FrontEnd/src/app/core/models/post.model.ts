

// -----------------------------------------------------------------------------------------------------
// @ Post

import { Item } from "../types/file-manager.types";
import { IPost } from "../types/post.types";
import { SocialChannel } from "./socialChannel.model";

// -----------------------------------------------------------------------------------------------------
export class Post implements Required<IPost>
{
    createdBy: string | null;
    lastModifiedBy: string | null;
    lastModifiedOn: string | null;
    deletedOn: string | null;
    deletedBy: string | null;

    id: string | null;
    message: string;
    photo: Item | null;
    icon: string | null;
    link: string | null;
    useRouter: boolean | null;
    lastActivity: string | null;
    socialChannels: SocialChannel[];
    // brandMembers: Member[];
    // publishing?: IList[];
    //role&permission
    // notifications:Inotification[];
    /**
     * Constructor
     */
    content?: string;

    images?: Item[] | null;
  

    createdAt?: string;
    updatedAt?: string | null;
    constructor(post: IPost) {

        this.createdBy = post.createdBy || null;
        this.lastModifiedBy = post.lastModifiedBy || null;
        this.lastModifiedOn = post.lastModifiedOn || null;
        this.deletedOn = post.deletedOn || null;
        this.deletedBy = post.deletedBy || null;
        this.id = post.id || null;

        this.photo = post.photo || null;
        this.link = "brand/channel";
        this.lastActivity = post.lastActivity || null;
        this.images = [];

        // this.socialChannels = [];
        // this.brandMembers = [];
        // this.brandMembers = [];

        // SocialChannels
        // if ( brand.socialChannels )
        // {
        //     this.socialChannels = brand.socialChannels.map((list) => {
        //         if ( !(list instanceof SocialChannel) )
        //         {
        //             return new SocialChannel(list);
        //         }

        //         return list;
        //     });
        // }

        // Members
        // if ( brand.brandMembers )
        // {
        //     this.brandMembers = brand.brandMembers.map((member) => {
        //         if ( !(member instanceof Member) )
        //         {
        //             return new Member(member);
        //         }

        //         return member;
        //     });
        // }

    }

}
// // -----------------------------------------------------------------------------------------------------
// // @ SocialChannel
// // -----------------------------------------------------------------------------------------------------
// export class SocialChannel implements Required<ISocialChannel>
// {
//     id: string | null;
//     channelType: string;
//     icon: string | null;
//     connectedAs:string;
//     permissions:String[];
//      /**
//      * Constructor
//      */
//      constructor(socialChannel: ISocialChannel)
//      {
//         this.id = socialChannel.id || null;
//         this.channelType = socialChannel.channelType;
//         this.icon = socialChannel.icon;
//         this.connectedAs = socialChannel.connectedAs;
//         this.permissions = [];
//      }
// }
// // -----------------------------------------------------------------------------------------------------
// // @ Member
// // -----------------------------------------------------------------------------------------------------
// export class Member implements Required<IMember>
// {
//     id: string | null;
//     name: string;
//     avatar: string | null;

//     /**
//      * Constructor
//      */
//     constructor(member: IMember)
//     {
//         this.id = member.id || null;
//         this.name = member.name;
//         this.avatar = member.avatar || null;
//     }
// }
export interface Label {
    id?: string;
    title?: string;
}
export interface Task {
    id?: string;
    content?: string;
    completed?: string;
}

