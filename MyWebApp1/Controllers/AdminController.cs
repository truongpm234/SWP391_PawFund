using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_adminService.GetUsers());
        }

        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUser(int id)
        {
            var user = _adminService.GetUser(id);
            if (user != null)
                return Ok(user);
            else
                return NoContent();
        }

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
                //bỏ "Bearer " để lấy token
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // xu li token lay thong tin
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                // Lấy userId từ claim trong token
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized("UserId not found in token.");
                }

                var userId = int.Parse(userIdClaim.Value);

                // Tìm người dùng từ cơ sở dữ liệu dựa trên userId
                var user = _adminService.GetUser(userId);

                if (user != null)
                {
                    var userInfo = new
                    {
                        userId = user.UserId,
                        Fullname = user.Fullname,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Username = user.Username,
                        RoleId = user.RoleId,
                        //RoleName = user.RoleName
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

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("remove-user-role/{userId}")]
        public async Task<IActionResult> RemoveUserRole(int userId)
        {
            try
            {
                await _adminService.RemoveUserRole(userId);
                return Ok($"User role for user ID {userId} has been removed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-requests")]
        public async Task<ActionResult<List<User>>> GetAllManagerRequests()
        {
            var requests = await _adminService.GetAllManagerRequests();
            return Ok(requests);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("approveManagerRequest/{userId}")]
        public async Task<ActionResult<User>> ApproveManagerRequest(int userId)
        {
            try
            {
                var user = await _adminService.ApproveManagerRequest(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
