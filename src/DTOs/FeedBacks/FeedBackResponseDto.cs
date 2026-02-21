namespace pingword.src.DTOs.FeedBacks
{
    public record FeedBackResponseDto
    {
        public Guid Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 
    }
}
