

// -----------------------------------------------------------------------------------------------------
// @ Post

import { DateTime } from "luxon";
import { Item } from "../types/file-manager.types";
import { IInstagramPost } from "../types/instagram-post.types";
import { MediaType } from "../enumerations/MedyaType";
import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { InstagramChannel } from "./instagram-channel";
export class InstagramPost{

    Id :string
    Caption  :string
    Media_url :string // video or image                  
    PublicationStatusEnum :PublicationStatusEnum
    ScheduleTime  :DateTime               
    UpdatedTime  :DateTime
    Media_type :MediaType
    InstagramChannelDto? :InstagramChannel 
    InstagramChannelId: string
    CreatedTime :DateTime
    InstagramPostId :string
    PostId :string
   // PostDto? :PostDto 


}

// -----------------------------------------------------------------------------------------------------
// export class InstagramPost implements Required<IInstagramPost>
// {
//     caption(imageURL: (imageURL: any, caption: any) => import("rxjs").Observable<any>, caption: any): import("rxjs").Observable<any> {
//       throw new Error('Method not implemented.');
//     }
//     imageURL(imageURL: any, caption: any): import("rxjs").Observable<any> {
//       throw new Error('Method not implemented.');
//     }
//     createdBy: string | null;
//     lastModifiedBy: string | null;
//     lastModifiedOn: string | null;
//     deletedOn: string | null;
//     deletedBy: string | null;
//     // icon: "string",
//     // link: "www.consultim-it.com",
//     // message: "test"



//     id: string | null;
//     title: string;
//     postContent: string;
//     message: string;
//     description: string | null;
//     photo: Item | null;
//     coverPhoto: string | null;
//     icon: string | null;
//     link: string | null;
//     useRouter: boolean | null;
//     lastActivity: string | null;
    // socialChannels: SocialChannel[];
    // brandMembers: Member[];
    // publishing?: IList[];
    //role&permission
    // notifications:Inotification[];
    /**
     * Constructor
     */
    // content?: string;
    // tasks?: Task[];
    // images?: Item[] | null;
    // labels?: Label[];
    // archived?: boolean;
    // createdAt?: string;
    // updatedAt?: string | null;
    // constructor(post: IInstagramPost) {

    //     this.createdBy = post.createdBy || null;
    //     this.lastModifiedBy = post.lastModifiedBy || null;
    //     this.lastModifiedOn = post.lastModifiedOn || null;
    //     this.deletedOn = post.deletedOn || null;
    //     this.deletedBy = post.deletedBy || null;
    //     this.id = post.id || null;
    //     this.title = post.title;
    //     this.postContent = post.postContent;
    //     this.description = post.description || null;
    //     this.photo = post.photo || null;
    //     this.link = "brand/channel";
    //     this.lastActivity = post.lastActivity || null;
    //     this.images = [];

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

//     }

// }
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

