import { IAuditableEntityBase } from "app/core/types/shared/auditableEntityBase.types";

export class  AuditableEntityBase implements Required<IAuditableEntityBase> {
    id: string | null;
    createdBy: string | null;
    createdOn: Date | null;
    lastModifiedBy: string | null;
    lastModifiedOn: string | null;
    deletedOn: string | null;
    deletedBy: string | null;

    //  constructor(entity: IAuditableEntityBase) {
    // //     this.id = entity.id || null;
    //     this.createdBy = entity.createdBy || null;
    //     this.createdOn = entity.createdOn || null;
    //     this.lastModifiedBy = entity.lastModifiedBy || null;
    //     this.lastModifiedOn = entity.lastModifiedOn || null;
    //     this.deletedOn = entity.deletedOn || null;
    //     this.deletedBy = entity.deletedBy || null;
    //  }
}



