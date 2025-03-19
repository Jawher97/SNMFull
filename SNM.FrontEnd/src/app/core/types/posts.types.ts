import { PublicationStatusEnum } from "../enumerations/PublicationStatusEnum";
import { IAuditableEntityBase } from "./shared/auditableEntityBase.types";
// import { ISocialChannel } from "./socialChannel.types";

export interface IPosts extends IAuditableEntityBase {
    caption: string;
    description?: string | null;
    publicationDate?: Date | null;
    publicationStatus?: PublicationStatusEnum | null;
    socialChannels?: {
        id?: string | null;
        displayName?: string | null;
        photo?: string | null;
        brandId?: string | null;
        channelTypeId?: string | null;
    }[];
}

