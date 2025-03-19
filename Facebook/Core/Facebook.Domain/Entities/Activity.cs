using SNS.Facebook.Domain.Common;

namespace SNS.Facebook.Domain.Entities
{
    public class Activity : EntityBase<Guid>
    {
        public string icon { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public DateTime? date { get; set; }
        public string extraContent { get; set; }
        public string linkedContent { get; set; }
        public string link { get; set; }
        public bool? useRouter { get; set; }
    }

}
