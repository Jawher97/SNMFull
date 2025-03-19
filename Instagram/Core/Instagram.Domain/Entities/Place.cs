using SNM.Instagram.Domain.Common;

namespace SNM.Instagram.Domain.Entities
{
    public class Place : EntityBase<Guid>
    {
        public string PlaceId { get; set; }
        // public Location Location { get; set; }
        public string Name { get; set; }
    }
}
