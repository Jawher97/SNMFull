import { Item } from "./file-manager.types";

export interface IPost {
    id?: string | null;

    photo?: Item | null;

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



