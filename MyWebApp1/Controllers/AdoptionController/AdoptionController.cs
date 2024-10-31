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
                // Lấy thông tin user từ JWT
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                // lay userId từ claim
                var userId = int.Parse(userIdClaim.Value);

                // Xử lý yêu cầu với userId
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
                // Lấy thông tin user từ JWT token trong HttpContext
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

    }
}


