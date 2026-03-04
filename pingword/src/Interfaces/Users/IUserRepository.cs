using pingword.src.DTOs.Users;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;
using pingword.src.Models.Users;

namespace pingword.src.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(string id);
        Task<Study?> GetStudyByUserId(string UserId);
        IQueryable<Notification> GetUserNotificationsQuery(string userId);
    }
}
