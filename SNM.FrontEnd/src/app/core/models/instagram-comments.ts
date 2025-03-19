import { DateTime } from "luxon";

export interface InstagramComments {
    isLiked: boolean;
    
    id: string;
    text : string;
    timestamp: DateTime ;
    like_count : number; 
}

