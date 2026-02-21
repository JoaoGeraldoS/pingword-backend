using pingword.src.Models.FeedBacks;

namespace pingword.src.Interfaces.FeedBacks
{
    public interface IFeedBackRepository
    {
        Task AddFeedBack(FeedBack feedBack);
        Task<List<FeedBack>> GetAllFeedBacks();

        Task<FeedBack?> GetFeedBackById(Guid id);
        Task RemoveFeedBack(FeedBack feedBack);
    }
}
