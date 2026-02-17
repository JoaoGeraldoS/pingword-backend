using pingword.src.Models.StudyState;

namespace pingword.src.Interfaces.StudyState
{
    public interface IStudyStateRepository
    {
        Task<Study?> GetStudyByUserId(string UserId);
        Task<List<Study>> GetStateActiveAsync(DateTime limitDate);
        Task AddStateAsync(Study study);
        Task SaveChangesAsync();
    }
}
