using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Models;
using MyWebApp1.Services;

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

        [HttpGet("GetInformationShelter/{id}")]
        public IActionResult GetInformationShelter(int id)
        {
            var shelter = _shelterService.GetShelterById(id);
            if (shelter == null)
            {
                return NotFound("Shelter not found.");
            }

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
        }

        [HttpGet("GetAllShelters")]
        public async Task<IActionResult> GetAllShelters()
        {
            var sheltersWithPets = await _shelterService.GetAllSheltersAsync();
            return Ok(sheltersWithPets);
        }

    }
}