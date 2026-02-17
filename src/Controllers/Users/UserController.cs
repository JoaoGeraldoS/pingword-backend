using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Users;
using pingword.src.Interfaces.Users;

namespace pingword.src.Controllers.Users
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

        [HttpGet("{userId}/performance")]
        public async Task<ActionResult<UserPerformaceDto>> GetUserPerformance(string userId)
        {
            var response = await _userService.GetUserPerformanceAsync(userId);
            return Ok(response);
        }
    }
}
