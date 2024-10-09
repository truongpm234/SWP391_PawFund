using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegisterUserDTO userDTO)
        {
            try
            {
                // Gọi hàm Register từ UserService, truyền vào userDTO
                var result = _userService.Register(userDTO);
                return Ok(result);
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
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);
            if (user != null)
                return Ok(user);
            else
                return NoContent();
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateProfile/{userId}")]
        public IActionResult UpdateProfile(int userId, UpdateProfileDTO userDTO)
        {
            try
            {
                var result = _userService.UpdateProfile(userId, userDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("GetCurrentLoginUser")]
        public IActionResult GetCurrentLoginUser()
        {
            // Lấy token từ header Authorization
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }

            try
            {
                // Bỏ wa "Bearer " để lấy token
                var token = authHeader.Substring("Bearer ".Length).Trim();

                //  lấy thông tin token
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                // Lấy userId từ claim
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized("UserId not found in token.");
                }

                // Lấy userId
                var userId = int.Parse(userIdClaim.Value);

                // Tìm người dùng từ cơ sở dữ liệu dựa trên userId
                var user = _userService.GetUser(userId);

                if (user != null)
                {
                    // Trả thông tin người dùng
                    var userInfo = new
                    {
                        Fullname = user.Fullname, 
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address
                    };
                    return Ok(userInfo);
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing request: " + ex.Message);
            }
        }
    }
}
