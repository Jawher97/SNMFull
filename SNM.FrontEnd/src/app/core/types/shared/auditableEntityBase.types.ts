export interface IAuditableEntityBase {
    id?: string | null;
    createdBy?: string| null;
    createdOn?: Date | null;
    lastModifiedBy?: string| null;
    lastModifiedOn?: string| null;
    deletedOn?: string| null;
    deletedBy?: string| null;
}



