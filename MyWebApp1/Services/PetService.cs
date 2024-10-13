using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models;
using MyWebApp1.Payload.Request;
using MyWebApp1.Payload.Response;

namespace MyWebApp1.Services
{
    public class PetService
    {
        private readonly MyDbContext _context;

        public PetService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Pet> AddNewPet(Models.Pet pet)
        {
            var Addpet = new Models.Pet
            {
                PetName = pet.PetName,
                PetType = pet.PetType,
                Age = pet.Age,
                Gender = pet.Gender,
                Address = pet.Address,
                MedicalCondition = pet.MedicalCondition,
                ContactPhoneNumber = pet.ContactPhoneNumber,
                ContactEmail = pet.ContactEmail
            };

            await _context.Pets.AddAsync(Addpet);
            await _context.SaveChangesAsync();
            return pet;
        }

        public async Task<List<PetResponse>> GetAllPets()
        {
            var pets = await _context.Pets.ToListAsync();
            var petList = new List<PetResponse>();
            foreach (var item in pets)
            {
                var petResponse = new PetResponse()
                {
                    PetId = item.PetId,
                    PetName = item.PetName,
                    Address = item.Address,
                    MedicalCondition = item.MedicalCondition,
                    ContactPhoneNumber = item.ContactPhoneNumber,
                    ContactEmail = item.ContactEmail,
                    Age = item.Age,
                    Gender = item.Gender,
                    PetType = item.PetType,
                    IsAdopted = item.IsAdopted,
                    IsApproved = item.IsApproved
                };
                petList.Add(petResponse);
            }
            return petList;
        }

        public async Task<PetResponse> GetPet(int petId)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(pet => pet.PetId == petId);
            return new PetResponse()
            {
                PetId = pet.PetId,
                PetName = pet.PetName,
                Address = pet.Address,
                MedicalCondition = pet.MedicalCondition,
                ContactPhoneNumber = pet.ContactPhoneNumber,
                ContactEmail = pet.ContactEmail,
                Age = pet.Age,
                Gender = pet.Gender,
                PetType = pet.PetType,
                IsAdopted = pet.IsAdopted,
                IsApproved = pet.IsApproved
            };
        }

        public async Task DeletePet(int id)
        {
            _context.Pets.Remove(new Pet()
            {
                PetId = id
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePet(PetEditRequest request)
        {
            var pet = new Pet()
            {
                PetId = request.PetId,
                PetName = request.PetName,
                Address = request.Address,
                MedicalCondition = request.MedicalCondition,
                ContactPhoneNumber = request.ContactPhoneNumber,
                ContactEmail = request.ContactEmail,
                Age = request.Age,
                Gender = request.Gender,
                PetType = request.PetType,
                IsAdopted = request.IsAdopted,
                IsApproved = request.IsApproved
            };
            _context.Attach(pet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }


}
