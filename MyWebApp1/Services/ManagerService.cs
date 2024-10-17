using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public class ManagerService
    {
        private readonly MyDbContext _context;

        public ManagerService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Pet> ApprovePet(int petId)
        {
            // Find the pet in the database
            var existingPet = await _context.Pets.FindAsync(petId);
            if (existingPet == null)
            {
                throw new Exception("Pet not found");
            }

            // IsApprove to true
            existingPet.IsApproved = true;

            // Update pet status
            _context.Entry(existingPet).State = EntityState.Modified;

            // Save database
            await _context.SaveChangesAsync();

            return existingPet;
        }

        public async Task DeletePet(int petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pet>> GetAllPets()
        {
            return await _context.Pets.ToListAsync();
        }
    }
}
