using pingword.src.Models.StudyState;

namespace pingword.src.Interfaces.StudyState
{
    public interface IStudyStateRepository
    {
        Task<Study?> GetStudyByUserId(string UserId);
    }
}
