﻿using FluentEmail.Core;
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

        // Kết hợp cả HttpGet và Route thành một
        [HttpGet("GetInformationShelter/{id}")]
        public IActionResult GetInformationShelter(int id)
        {
            var shelter = _shelterService.GetShelterById(id);
            if (shelter == null)
            {
                return NotFound("Shelter not found.");
            }

            // Trả về thông tin shelter
            var shelterInfo = new
            {
                ShelterId = shelter.ShelterId,
                ShelterName = shelter.ShelterName,
                ShelterLocation = shelter.ShelterLocation,
                Capacity = shelter.Capacity,
                Contact = shelter.Contact,
                Email = shelter.Email,
                OpeningClosing = shelter.OpeningClosing
            };

            return Ok(shelterInfo);
        }

        [HttpGet("GetAllShelters")]
        public IActionResult GetAllShelters()
        {
            IEnumerable<Shelter> shelters = _shelterService.GetAllShelters();
            return Ok(shelters);
        }
    }
}