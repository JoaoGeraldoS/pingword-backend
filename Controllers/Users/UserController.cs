using Microsoft.AspNetCore.Mvc;
using pingword.DTOs.Users;
using pingword.Interfaces.Users;

namespace pingword.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserRegisterResponseDto>> RegisterUser([FromBody] UserRegisterRequestDto request)
        {
            var response = await _userService.RegisterUserAsync(request);
            return Ok(response);
        }
    }
}
