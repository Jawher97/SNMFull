using SNM.LinkedIn.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Company { get; set; }
        public string? About { get; set; }
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public string? Status { get; set; }
        public string? Avatar { get; set; }
        public bool? RemeberMe { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }

        public bool? isActivated { get; set; } = false;
        public string? ActivateToken { get; set; }
        public DateTime ActivateTokenExpiry { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }






    }
}
