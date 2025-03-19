import { DateTime } from "luxon"
import { LinkedInInsight } from "./linkedin-insight"

export interface LinkedinComment {

    commentId: string 
    activityUrn: string 
    actorUrn: string
    actorUserName: string 
    actorprofilelink: string
    actorheadline: string
    comment: string
    created_at: DateTime
    Updated_at: DateTime
    parentId: null | string
    insight: LinkedInInsight
    subCommentsList:LinkedinComment[]

}
