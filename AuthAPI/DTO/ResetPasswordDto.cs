namespace AuthAPI.DTO
{
    public record ResetPasswordDto
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? EmailToken { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
