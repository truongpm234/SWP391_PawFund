using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Payload;
using MyWebApp1.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _staffService;
        private readonly AdoptionService _adoptionService;


        public StaffController(StaffService staffService, AdoptionService adoptionService)
        {
            _staffService = staffService;
            _adoptionService = adoptionService;
        }

        [Authorize(Policy = "ManagerOrStaff")]
        [HttpPut("update-pet-by-staff/{petId}")]
        public async Task<IActionResult> UpdatePet(int petId, [FromBody] PetUpdateDTO updatedPet)
        {
            try
            {
                var result = await _staffService.UpdatePet(petId, updatedPet);
                return result ? Ok("Pet updated successfully.") : BadRequest("Failed to update pet.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "StaffOnly")]
        [HttpPost("AddNewPet-by-staff")]
        public async Task<IActionResult> AddNewPet([FromBody] AddNewPetDTO newPetDTO)
        {
            try
            {
                // Lấy UserId của staff từ token
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var result = await _staffService.AddNewPetForStaff(newPetDTO, userId);

                return Ok(new { Message = "Pet added to shelter successfully", Pet = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Policy = "ManagerOrStaff")]
        [HttpGet("get-pet-in-shelter-by-staff")]
        public async Task<IActionResult> GetPetsByShelterForStaff()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                var pets = await _staffService.GetPetsByShelterForStaff(userId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOrStaff")]
        [HttpPut("approve-adoption-request/{adoptionId}")]
        public async Task<IActionResult> ApproveAdoptionByStaff(int adoptionId, [FromBody] ApproveAdoptionRequestDto request)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                var userId = int.Parse(userIdClaim.Value);

                var result = await _staffService.ApproveAdoptionByStaff(userId, adoptionId, request);
                return result ? Ok("Adoption approved.") : BadRequest("Failed to approve adoption.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = "ManagerOrStaff")]
        [HttpGet("get-all-adoptions-by-staff-and-manager")]
        public IActionResult GetAdoptions()
        {
            try
            {
                var adoptions = _staffService.GetAdoptions();
                return Ok(adoptions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
