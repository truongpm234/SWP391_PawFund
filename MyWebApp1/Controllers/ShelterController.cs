using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Services;
using System.Threading.Tasks;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
    {
        private readonly IShelterService _shelterService;

        public ShelterController(IShelterService shelterService)
        {
            _shelterService = shelterService;
        }

        [HttpGet("GetAllShelters")]
        public async Task<IActionResult> GetAllShelters()
        {
            var sheltersWithPets = await _shelterService.GetAllSheltersWithPetsAsync();
            return Ok(sheltersWithPets);
        }

        [HttpGet("GetInformationShelter/{id}")]
        public async Task<IActionResult> GetInformationShelter(int id)
        {
            var shelterWithPets = await _shelterService.GetShelterWithPetsByIdAsync(id);
            if (shelterWithPets == null)
            {
                return NotFound("Shelter not found.");
            }

            return Ok(shelterWithPets);
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("CreateShelter-by-Manager")]
        public async Task<IActionResult> CreateShelter([FromBody] ShelterCreateDTO shelterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shelter = await _shelterService.CreateShelterAsync(shelterDto);
            return CreatedAtAction(nameof(GetInformationShelter), new { id = shelter.ShelterId }, shelter);
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPut("UpdateShelter-by-Manager/{id}")]
        public async Task<IActionResult> UpdateShelter(int id, [FromBody] ShelterCreateDTO shelterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shelterService.UpdateShelterAsync(id, shelterDto);
            if (!result)
            {
                return NotFound("Shelter not found.");
            }

            return Ok(shelterDto);
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpDelete("DeleteShelter-by-Manager/{id}")]
        public async Task<IActionResult> DeleteShelter(int id)
        {
            var result = await _shelterService.DeleteShelterAsync(id);
            if (!result)
            {
                return NotFound("Shelter not found.");
            }

            return Ok();
        }
    }
}