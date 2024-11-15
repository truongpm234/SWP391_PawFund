using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("list-approved-requests")]
        public async Task<IActionResult> GetApprovedRequests()
        {
            var approvedRequests = await _managerService.GetApprovedRequestsAsync();
            return Ok(approvedRequests);
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("list-pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var pendingRequests = await _managerService.GetPendingRequestsAsync();
            return Ok(pendingRequests);
        }
        
        [Authorize(Policy = "ManagerOrStaff")]
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
        [HttpGet("get-all-pet-by-manager")]
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

//        var staffId = int.Parse(staffIdClaim.Value);

//        var approvedPet = await _managerService.ApprovePet(petId, staffId, shelterId);

//        return Ok(approvedPet);
//    }
//    catch (UnauthorizedAccessException ex)
//    {
//        return Forbid(ex.Message);
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, "An internal error occurred: " + ex.Message);
//    }
//}