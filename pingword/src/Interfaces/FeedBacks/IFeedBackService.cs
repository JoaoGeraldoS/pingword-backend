using pingword.src.DTOs.FeedBacks;

namespace pingword.src.Interfaces.FeedBacks
{
    public interface IFeedBackService
    {
        Task AddFeedBackAsync(FeedBackRequestDto feedBackRequestDto);
        Task<List<FeedBackResponseDto>> GetAllFeedBacks();
        Task<FeedBackResponseDto> GetFeedBackByIdAsync(Guid id);
        Task RemoveFeedBackAsync(Guid id);
    }
}
