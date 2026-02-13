using Microsoft.EntityFrameworkCore;
using pingword.Data;
using pingword.Interfaces.Users;
using pingword.Models.Users;

namespace pingword.Repositories.Users
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
