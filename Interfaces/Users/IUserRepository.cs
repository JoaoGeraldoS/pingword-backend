using pingword.Models.Users;

namespace pingword.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(string id);
    }
}
