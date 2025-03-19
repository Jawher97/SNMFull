import { Item } from "./file-manager.types";

export interface IInstagramPost {
    id?: string | null;
    title: string;
    postContent: string;
    description?: string | null;
    photo?: Item | null;
    coverPhoto?: string | null;
    icon?: string| null;
    link?: string| null;
    useRouter?: boolean| null;
    lastActivity?: string | null;  

    createdBy?: string| null;
    lastModifiedBy?: string| null;
    lastModifiedOn?: string| null;
    deletedOn?: string| null;
    deletedBy?: string| null;
}



