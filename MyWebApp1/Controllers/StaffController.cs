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

        [Authorize(Policy = "StaffOnly")]
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
        [HttpGet("get-pet-in-shelter-by-staff")]
        public async Task<IActionResult> GetPetsByShelterForStaff()
        {
            try
            {
                // Lấy userId từ token (claims)
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                var pets = await _staffService.GetPetsByShelterForStaff(userId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "StaffOnly")]
        //[HttpPut("approve-adoption-by-staff/{userId}/{adoptionId}")]
        //public async Task<IActionResult> ApproveAdoptionByStaff(int userId, int adoptionId)
        //{
        //    try
        //    {
        //        var result = await _staffService.ApproveAdoptionByStaff(userId, adoptionId);
        //        return result ? Ok("Adoption approved.") : BadRequest("Failed to approve adoption.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [Authorize(Policy = "StaffOnly")]
        [HttpPut("approve-adoption/{adoptionId}")]
        public async Task<IActionResult> ApproveAdoptionByStaff(int adoptionId, [FromBody] ApproveAdoptionRequestDto request)
        {
            try
            {
                // Lấy userId từ JWT token
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


        [Authorize]
        [HttpGet("get-all-adoptions-by-staff")]
        public IActionResult GetAdoptions()
        {
            try
            {
                // Gọi service để lấy danh sách adoption (loại bỏ userId)
                var adoptions = _staffService.GetAdoptions();
                return Ok(adoptions);
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
