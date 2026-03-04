using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using pingword.src.Repositories.Users;
using pingword.Tests.Data;

namespace pingword.Tests.Users.Repository
{
    public class GetUserByIdTest : DatabaseTest
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdTest()
        {
            _userRepository = new UserRepository(_context);
        }

        [Fact(DisplayName ="Deve retornar um usuario")]
        public async Task Deve_retornar_usuario_com_sucesso()
        {
            var user = new User
            {
                Name = "Test User",
                Language = "en",
                TimeZone = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var existsUser = await _userRepository.GetUserById(user.Id);

            Assert.NotNull(existsUser);
            Assert.Equal(user.Name, existsUser.Name);

        }

        [Fact(DisplayName = "Deve retornar null para usuario inexistente")]
        public async Task Deve_retornar_null_para_usuario_inexistente()
        {
            
            var id = Guid.NewGuid().ToString();
            var existsUser = await _userRepository.GetUserById(id);

            Assert.Null(existsUser);
            
        }
    }
}
