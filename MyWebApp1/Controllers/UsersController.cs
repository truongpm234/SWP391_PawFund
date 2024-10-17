using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Payload;
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
        private readonly AdminService _adminService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        [Route("AddNewPet")]
        public async Task<IActionResult> AddNewPet([FromBody] Pet pet)
        {
            try
            {
                var newPet = await _userService.AddNewPet(pet);
                return Ok(newPet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-approved-pet")]
        public async Task<IActionResult> GetAllApprovedPets()
        {
            var pets = await _userService.GetAllApprovedPets();

            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Get all approved pets successful!",
                Data = pets
            });
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

        //[Authorize]
        //[HttpGet("get-current-user")]
        //public IActionResult GetCurrentUserLogin(int id)
        //{
        //    var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(new ApiResponse
        //        {
        //            StatusCode = 401,
        //            Message = "User not authenticated",
        //            Data = null
        //        });
        //    }

        //    var user = _adminService.GetUser(id);

        //    if (user == null)
        //    {
        //        return NotFound(new ApiResponse
        //        {
        //            StatusCode = 404,
        //            Message = "User not found",
        //            Data = null
        //        });
        //    }

        //    return Ok(new ApiResponse
        //    {
        //        StatusCode = 200,
        //        Message = "User retrieved successfully!",
        //        Data = user
        //    });
        //}

        [Authorize]
        [HttpPut]
        [Route("UpdateProfile/{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, UpdateProfileDTO userDTO)
        {
            try
            {
                // Gọi phương thức trong UserService để cập nhật thông tin
                var result = await _userService.UpdateProfile(userId, userDTO, HttpContext.User);
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
        [HttpPost("request-role-manager")]
        public async Task<ActionResult<User>> RequestManagerRole([FromBody] RoleRequestDTO roleRequest)
        {
            try
            {
                // Ensure the requested username is not null or empty.
                if (string.IsNullOrEmpty(roleRequest.RequestedUsername))
                {
                    return BadRequest("Requested username cannot be null or empty.");
                }

                // Retrieve the logged-in username from the RoleRequestDTO
                var loggedInUsername = roleRequest.LoggedInUsername;

                // Call the service method to handle the request, passing the requested username and the logged-in username.
                var user = await _userService.RequestManagerRole(roleRequest.RequestedUsername, loggedInUsername);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
