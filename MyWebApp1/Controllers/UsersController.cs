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

        [Authorize(Policy = "UserOrStaff")]
        [HttpPost]
        [Route("AddNewPet")]
        public async Task<IActionResult> AddNewPet([FromBody] AddNewPetDTO newPetDTO)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var pet = new Models.Pet
                {
                    PetName = newPetDTO.PetName,
                    PetType = newPetDTO.PetType,
                    Age = newPetDTO.Age,
                    Gender = newPetDTO.Gender,
                    Address = newPetDTO.Address,
                    MedicalCondition = newPetDTO.MedicalCondition,
                    Description = newPetDTO.Description,
                    Color = newPetDTO.Color,
                    Size = newPetDTO.Size,
                    ContactPhoneNumber = newPetDTO.ContactPhoneNumber,
                    ContactEmail = newPetDTO.ContactEmail,
                    PetCategoryId = newPetDTO.PetCategoryId,
                    CreatedAt = DateTime.Now,
                    IsAdopted = false,
                    IsApproved = false
                };

                if (newPetDTO.PetImages != null && newPetDTO.PetImages.Any())
                {
                    pet.PetImages = newPetDTO.PetImages.Select(imageDto => new PetImage
                    {
                        ImageDescription = imageDto.ImageDescription,
                        ImageUrl = imageDto.ImageUrl,
                        IsThumbnailImage = imageDto.IsThumbnailImage
                    }).ToList();
                }

                var addedPet = await _userService.AddNewPet(pet, userId);

                return Ok(addedPet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [HttpPost("request-role-manager")]
        public async Task<ActionResult<User>> RequestManagerRole()
        {
            try
            {
                var user = await _userService.RequestManagerRole();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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