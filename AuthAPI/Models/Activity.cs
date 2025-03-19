using static System.Net.Mime.MediaTypeNames;

namespace AuthAPI.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string? icon { get; set; }
        public string? image { get; set; }
        public string? description { get; set; }
        public DateTime? date { get; set; }
        public string? extraContent { get; set; }
        public string? linkedContent { get; set; }
        public string? link { get; set; }
        public bool? useRouter { get; set; }

        public string? userEmail { get; set; }
    }
}

