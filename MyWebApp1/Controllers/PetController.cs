﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Models;
using MyWebApp1.Payload;
using MyWebApp1.Payload.Request;
using MyWebApp1.Services;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        // Khai báo biến _petService
        private readonly PetService _petService;

        // Khởi tạo _petService thông qua constructor
        public PetsController(PetService petService)
        {
            _petService = petService;
        }

        [Authorize]
        [HttpPost]
        [Route("AddNewPet")]
        public async Task<IActionResult> AddNewPet([FromBody] Pet pet)
        {
            try
            {
                var newPet = await _petService.AddNewPet(pet);
                return Ok(newPet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPets()
        {
            var pets = await _petService.GetAllPets();
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Get all pets successful!",
                Data = pets
            });
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetPetById(int id)
        {
            var pet = await _petService.GetPet(id);
            return Ok(new ApiResponse()
            {
                StatusCode = 200,
                Message = "Get pet successful!",
                Data = pet
            });
        }

        [Authorize]
        [HttpDelete("delete-pet")]
        public async Task<IActionResult> DeletePet(int id)
        {
            try
            {
                await _petService.DeletePet(id);
                return Ok(new ApiResponse()
                {
                    StatusCode = 200,
                    Message = "Delete pet successful!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("update-pet")]
        public async Task<IActionResult> UpdatePet(Pet pet)
        {
            try
            {
                await _petService.UpdatePet(pet);
                return Ok(new ApiResponse()
                {
                    StatusCode = 200,
                    Message = "Update successful!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
