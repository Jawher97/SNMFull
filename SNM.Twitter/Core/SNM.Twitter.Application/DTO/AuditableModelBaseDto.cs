using AutoMapper;


/*
 * C'est une classe de base abstraite pour les DTO (Data Transfer Objects) qui représentent des modèles de données vérifiables
 * Un modèle de données auditable est un modèle qui permet de savoir qui l'a créé, modifié et/ou supprimé et à quel moment.
 */
namespace SNM.Twitter.Application.DTO
{
    public abstract class AuditableModelBaseDto : Profile
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
