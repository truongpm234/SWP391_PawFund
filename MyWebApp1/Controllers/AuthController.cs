using Microsoft.AspNetCore.Mvc;
using MyWebApp1.DTO;
using MyWebApp1.Services;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterUserDTO userDTO)
        {
            try
            {
                var result = _userService.Register(userDTO);
                if (result == "User registered successfully.")
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO login)
        {
            try
            {
                var token = _userService.Login(login);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
