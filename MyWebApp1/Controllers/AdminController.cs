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

        [Authorize(Policy = "AdminOnly")]
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

        // Xóa quyền manager của user
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
