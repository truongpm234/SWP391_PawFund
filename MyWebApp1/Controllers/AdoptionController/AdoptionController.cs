using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionController : ControllerBase
    {
        private readonly AdoptionService _adoptionService;
        private readonly MyDbContext _context;

        public AdoptionController(AdoptionService adoptionService, MyDbContext context)
        {
            _adoptionService = adoptionService;
            _context = context;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("adoption-request/{petId}")]
        public IActionResult CreateAdoptionRequest(int petId, [FromBody] AdoptionRequestModel request)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                // Lấy userId từ claim
                var userId = int.Parse(userIdClaim.Value);

                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (!user.IsApprovedUser)
                {
                    return BadRequest("Your account is not approved for creating adoption requests. Please complete verification through the CreateUserApproveRequest API.");
                }

                bool success = _adoptionService.CreateAdoptionRequest(request, userId, petId);

                return success ? Ok("Adoption request created successfully.") : StatusCode(500, "Failed to create adoption request.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("user-adoption-requests")]
        public IActionResult GetMyAdoptionRequests()
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                var userId = int.Parse(userIdClaim.Value);

                // lay adoption list
                var userAdoptions = _adoptionService.GetAdoptionsByUser(userId);

                return Ok(userAdoptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendReminders()
        {
            try
            {
                // Gọi service để kiểm tra và gửi nhắc nhở cho các yêu cầu nhận nuôi
                await _adoptionService.CheckAndSendReminderAsync();

                return Ok("Reminders sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}


