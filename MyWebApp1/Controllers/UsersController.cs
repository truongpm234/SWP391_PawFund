using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Payload;
using MyWebApp1.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AdminService _adminService;
        private readonly MyDbContext _dbContext;

        public UsersController(UserService userService, AdminService adminService, MyDbContext myDbContext)
        {
            _userService = userService;
            _adminService = adminService;
            _dbContext = myDbContext;
        }


        [HttpGet("get-all-pet")]
        public async Task<IActionResult> GetAllApprovedPets()
        {
            try
            {
                var pets = await _userService.GetAllApprovedPets();

                return Ok(new ApiResponse()
                {
                    StatusCode = 200,
                    Message = "Get all approved pets successful!",
                    Data = pets
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = 400,
                    Message = "Error occurred while fetching approved pets.",
                    Data = null
                });
            }
        }

        [HttpGet("get-approved-pet-by-id")]
        public async Task<IActionResult> GetApprovedPetById(int id)
        {
            var pet = await _userService.GetApprovedPet(id);

            if (pet == null)
            { 
                return NotFound(new ApiResponse()
                {
                    StatusCode = 404,
                    Message = "Pet not found or not approved",
                    Data = null
                });
            }

            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Get approved pet successful!",
                Data = pet
            });
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateProfile-by-user")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDTO userDTO)
        {
            try
            {
                var result = await _userService.UpdateProfile(HttpContext.User, userDTO);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("create-request-approve-info-user")]
        public async Task<IActionResult> RequestApproval([FromBody] UserApproveRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }
            int userId = int.Parse(userIdClaim.Value);

            var approveId = await _userService.CreateUserApproveRequest(request, userId);
            if (approveId > 0)
            {
                return Ok(new { Message = "Request submitted successfully", ApproveId = approveId });
            }
            return BadRequest("Failed to submit request");
        }

        [HttpGet]
        [Route("GetPet-of-User")]
        public async Task<IActionResult> GetUserPets()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                Console.WriteLine($"User ID from token: {userId}");

                var userPets = await _userService.GetPetsByUserId(userId);

                if (userPets == null || !userPets.Any())
                {
                    return NotFound("No pets found for this user.");
                }

                return Ok(userPets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-pets-in-shelter/{shelterId}")]
        public async Task<IActionResult> GetPetsByShelter(int shelterId)
        {
            try
            {
                var pets = await _userService.GetPetsByShelter(shelterId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
