using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using pingword.src.DTOs.Users;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using pingword.src.Services.Users;

namespace pingword.Tests.Users.Services
{
    
    public class RegisterUserTests
    {
        
        private readonly Mock<UserManager<User>> _userManage;
        private readonly Mock<IUserRepository> _userRepository;

        public RegisterUserTests()
        {    
            var store = new Mock<IUserStore<User>>();

            _userManage = new Mock<UserManager<User>>(
                store.Object,
                null, null, null, null, null, null, null, null
                );
            _userRepository = new Mock<IUserRepository>();
            
        }

        [Fact(DisplayName = "Register new user success")]
        public async Task Register_new_user_success()
        {
            var validatorMock = new Mock<IValidator<UserRegisterRequestDto>>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userRequest = new UserRegisterRequestDto
            {
                Username = "testuser",
                Email = "teste@gmail.com",
                Language = "en",
                Password = "Password!23",
            };

            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserRegisterRequestDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());


            _userManage.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManage.Setup(x => x.CreateAsync(It.IsAny<User>(), userRequest.Password)).ReturnsAsync(IdentityResult.Success);

            var service = new UserService(_userManage.Object, _userRepository.Object, loggerMock.Object, validatorMock.Object, null);

           
            var result = await service.RegisterUserAsync(userRequest);

            Assert.NotNull(result);
            Assert.Equal(userRequest.Username, result.Username);

        }
    }
}
