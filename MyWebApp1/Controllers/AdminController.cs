using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.DTO;
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
        private readonly UserService _userService;
        private readonly IEmailService _emailService;

        public AdminController(AdminService adminService, UserService userService, IEmailService emailService)
        {
            _adminService = adminService;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("GetCurrentLoginUser")]
        public IActionResult GetCurrentLoginUser()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }

            try
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized("UserId not found in token.");
                }

                var userId = int.Parse(userIdClaim.Value);

                // Tìm user
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
                        IsApprovedUser = user.IsApprovedUser,
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
        [HttpPost("changeRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
        {
            if (request.UserId <= 0 || request.RoleId <= 0)
            {
                return BadRequest("Invalid UserId or RoleId.");
            }

            var result = await _adminService.ChangeUserRole(request.UserId, request.RoleId);

            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("User role updated successfully.");
        }

        //[Authorize(Policy = "AdminOnly")]
        [HttpGet("get-pending-approve-requests-list")]
        public IActionResult GetPendingApproveRequests()
        {
            // lấy danh sách các yêu cầu chưa được duyệt
            var pendingRequests = _adminService.GetPendingApproveRequests();

            if (pendingRequests != null && pendingRequests.Any())
            {
                return Ok(new { Message = "Pending approve requests list", Data = pendingRequests });
            }
            else
            {
                return NotFound(new { Message = "No pending approve requests found" });
            }
        }


        //[Authorize(Policy = "AdminOnly")]
        [HttpPost("approve-info-user")]
        public async Task<IActionResult> ApproveUser(int approveId, bool isApproved)
        {
            try
            {
                var result = await _adminService.ApproveUser(approveId, isApproved);

                if (result)
                {
                    return Ok(new { Message = "User approval status updated successfully." });
                }
                else
                {
                    return NotFound(new { Message = "Approval record not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating approval status.", Details = ex.Message });
            }
        }


        //[Authorize(Policy = "AdminOnly")]
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

        

        //[Authorize(Policy = "AdminOnly")]
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

        //[Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-manager-requests")]
        public async Task<ActionResult<List<User>>> GetAllManagerRequests()
        {
            var requests = await _adminService.GetAllManagerRequests();
            return Ok(requests);
        }

        //[Authorize(Policy = "AdminOnly")]
        [HttpPost("approve-Manager-Request/{userId}")]
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

        //[Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _adminService.DeleteUser(userId);
                if (result)
                {
                    return Ok($"User with ID {userId} has been successfully deleted.");
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
