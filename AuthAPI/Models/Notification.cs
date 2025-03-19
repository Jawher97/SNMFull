using static System.Net.Mime.MediaTypeNames;

namespace AuthAPI.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string? icon { get; set; }
        public string? image { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? time { get; set; }
        public string? link { get; set; }
        public bool? useRouter { get; set; }
        public bool? read { get; set; }

        public string? userEmail { get; set; }
    }
}
