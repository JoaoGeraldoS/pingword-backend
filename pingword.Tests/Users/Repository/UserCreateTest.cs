using FluentValidation.Resources;
using Microsoft.AspNetCore.Identity;
using pingword.src.Models.Users;
using pingword.Tests.Data;
using SQLitePCL;

namespace pingword.Tests.Users.Repository
{
    public class UserCreateTest : DatabaseTest
    {
        

        [Fact(DisplayName ="Deve criar o usuario")]
        public void Deve_criar_usuario_com_sucessoAsync()
        {


            // Arrange
            var user = new User
            {
                Name = "Test User",
                Language = "en",
                TimeZone = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            Assert.Contains(_context.Users, u => u.Name == "Test User");

        }

    }
}