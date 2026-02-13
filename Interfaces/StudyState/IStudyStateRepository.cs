using pingword.Models.StudyState;

namespace pingword.Interfaces.StudyState
{
    public interface IStudyStateRepository
    {
        Task<Study?> GetStudyByUserId(string UserId);
    }
}
