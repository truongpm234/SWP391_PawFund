using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Payload;
using MyWebApp1.Services;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService _managerService;

        public ManagerController(ManagerService managerService)
        {
            _managerService = managerService;
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("approve-pet/{petId}")]
        public async Task<IActionResult> ApprovePet(int petId)
        {
            try
            {
                var approvedPet = await _managerService.ApprovePet(petId);
                return Ok(approvedPet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpDelete("delete-pet/{petId}")]
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
