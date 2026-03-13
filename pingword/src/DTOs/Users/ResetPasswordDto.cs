namespace pingword.src.DTOs.Users
{
    public record ResetPasswordDto
    {
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
