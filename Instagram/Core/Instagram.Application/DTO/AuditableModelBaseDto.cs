using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.DTO
{
    public abstract class AuditableModelBaseDto : ModelBaseDto
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
