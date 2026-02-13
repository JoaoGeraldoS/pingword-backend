namespace pingword.src.DTOs.Notifications
{
    public record NotificationRequestDto
    {
        public string Word { get; set; } = string.Empty;
    }
}
