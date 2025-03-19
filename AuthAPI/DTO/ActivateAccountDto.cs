namespace AuthAPI.DTO
{
    public class ActivateAccountDto
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? EmailToken { get; set; }

    }
}
