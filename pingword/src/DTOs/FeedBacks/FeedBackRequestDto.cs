namespace pingword.src.DTOs.FeedBacks
{
    public record FeedBackRequestDto
    {
        public string User { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
