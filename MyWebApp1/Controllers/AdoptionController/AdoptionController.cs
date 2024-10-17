using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Models;
using MyWebApp1.Services;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionController : ControllerBase
    {
        private readonly AdoptionService _adoptionService;

        public AdoptionController(AdoptionService adoptionService)
        {
            _adoptionService = adoptionService;
        }
        [Authorize(Policy = "UserOnly")]
        [HttpPost("request")]
        public IActionResult CreateAdoptionRequest([FromBody] AdoptionRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool success = _adoptionService.CreateAdoptionRequest(request);
                return success ? Ok("Adoption request created successfully.") : StatusCode(500, "Failed to create adoption request.");
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("get-all-adoptions")]
        public IActionResult GetAdoptions()
        {
            var adoptions = _adoptionService.GetAdoptions();
            return Ok(adoptions);
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPut("approve/{adoptionId}")]
        public IActionResult ApproveAdoption(int adoptionId)
        {
            bool success = _adoptionService.ApproveAdoption(adoptionId);
            return success ? Ok("Adoption approved.") : NotFound("Adoption not found.");
        }
    }
}
