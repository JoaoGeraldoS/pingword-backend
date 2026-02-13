using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;

namespace pingword.src.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
