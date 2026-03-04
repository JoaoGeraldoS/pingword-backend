namespace pingword.src.Models.FeedBacks
{
    public class FeedBack
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string User { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
