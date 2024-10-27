using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.DTO;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Payload;
using MyWebApp1.Services;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService _managerService;
        private readonly IEmailService emailService;
        private readonly AdminService adminService;
        private readonly ManagerService managerService;
        private readonly UserService _userService;

        public ManagerController(ManagerService managerService, IEmailService service, AdminService adminService)
        {
            _managerService = managerService;
            this.emailService = service;
            this.adminService = adminService;
        }

        //[Authorize(Policy = "ManagerOnly")]
        //[HttpPost("approve-pet/{petId}/{shelterId}")]
        //public async Task<IActionResult> ApprovePet(int petId, int shelterId)
        //{
        //    try
        //    {
        //        var staffIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        //        if (staffIdClaim == null)
        //        {
        //            return Unauthorized("Staff ID claim not found in token.");
        //        }

        //        // Parse staffId từ claim
        //        var staffId = int.Parse(staffIdClaim.Value);

        //        // Gọi service để duyệt thú cưng
        //        var approvedPet = await _managerService.ApprovePet(petId, staffId, shelterId);


        //        return Ok(approvedPet);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Forbid(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An internal error occurred.");
        //    }
        //}

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("approve-pet/{petId}/{shelterId}")]
        public async Task<IActionResult> ApprovePet(int petId, int shelterId)
        {
            try
            {
                var staffIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (staffIdClaim == null)
                {
                    return Unauthorized("Staff ID claim not found in token.");
                }

                // Parse staffId từ claim
                var staffId = int.Parse(staffIdClaim.Value);

                // Gọi service để duyệt thú cưng và gửi email
                var approvedPet = await _managerService.ApprovePet(petId, staffId, shelterId);

                return Ok(approvedPet);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpDelete("delete-pet-by-manager/{petId}")]
        public async Task<IActionResult> DeletePet(int petId)
        {
            try
            {
                await _managerService.DeletePet(petId);
                return Ok($"Pet with ID {petId} has been deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPets()
        {
            var pets = await _managerService.GetAllPets();
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Get all pets successful!",
                Data = pets
            });
        }
    }

}
