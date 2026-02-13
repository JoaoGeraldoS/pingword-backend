using pingword.src.Models.Users;

namespace pingword.src.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(string id);
    }
}
