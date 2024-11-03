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

<<<<<<< HEAD
        [HttpGet("GetInformationShelter/{id}")]
        public IActionResult GetInformationShelter(int id)
=======
        [HttpGet("GetAllShelters")]
        public async Task<IActionResult> GetAllShelters()
>>>>>>> origin/Dat1
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

<<<<<<< HEAD
            var shelterInfo = new
            {
                ShelterId = shelter.ShelterId,
                ShelterName = shelter.ShelterName,
                ShelterLocation = shelter.ShelterLocation,
                Capacity = shelter.Capacity,
                Contact = shelter.Contact,
                Email = shelter.Email,
                OpeningClosing = shelter.OpeningClosing,
                ShelterImage = shelter.ShelterImage,
                Description = shelter.Description

            };

            return Ok(shelterInfo);
=======
            return Ok(shelterWithPets);
>>>>>>> origin/Dat1
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("CreateShelter-by-Admin")]
        public async Task<IActionResult> CreateShelter([FromBody] ShelterCreateDTO shelterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shelter = await _shelterService.CreateShelterAsync(shelterDto);
            return CreatedAtAction(nameof(GetInformationShelter), new { id = shelter.ShelterId }, shelter);
        }

        [Authorize(Policy = "StaffOnly")]
        [HttpPut("UpdateShelter-by-staff/{id}")]
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

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("DeleteShelter-by-Admin/{id}")]
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
