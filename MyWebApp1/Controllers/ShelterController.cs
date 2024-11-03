using Microsoft.AspNetCore.Mvc;
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
    }
}
