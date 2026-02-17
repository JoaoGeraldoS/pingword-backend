namespace pingword.src.Interfaces.StudyState
{
    public interface IStudyStateService
    {
        Task RegisterIteractionAsync(string userId);
        Task<int> ProcessExpireStateAsync(DateTime limitDate);
    }
}
