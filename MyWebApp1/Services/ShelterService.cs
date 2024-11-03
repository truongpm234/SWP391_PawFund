using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public class ShelterService : IShelterService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userService;

        public ShelterService(MyDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<ShelterWithPetsDTO>> GetAllSheltersWithPetsAsync()
        {
            var shelters = await _context.Shelters.ToListAsync();
            var shelterWithPetsList = new List<ShelterWithPetsDTO>();

            foreach (var shelter in shelters)
            {
                var approvedPets = await _userService.GetPetsByShelter(shelter.ShelterId);

                shelterWithPetsList.Add(new ShelterWithPetsDTO
                {
                    ShelterId = shelter.ShelterId,
                    ShelterName = shelter.ShelterName,
                    ShelterLocation = shelter.ShelterLocation,
                    ApprovedPets = approvedPets,
                    Capacity = shelter.Capacity,
                    Contact = shelter.Contact,
                    Email = shelter.Email,
                    OpeningClosing = shelter.OpeningClosing,
                    ShelterImage = shelter.ShelterImage,
                    Description = shelter.Description
                });
            }

            return shelterWithPetsList;
        }

        public async Task<ShelterWithPetsDTO> GetShelterWithPetsByIdAsync(int shelterId)
        {
            var shelter = await _context.Shelters.FindAsync(shelterId);
            if (shelter == null) return null;

            var approvedPets = await _userService.GetPetsByShelter(shelterId);

            return new ShelterWithPetsDTO
            {
                ShelterId = shelter.ShelterId,
                ShelterName = shelter.ShelterName,
                ShelterLocation = shelter.ShelterLocation,
                ApprovedPets = approvedPets,
                Capacity = shelter.Capacity,
                Contact = shelter.Contact,
                Email = shelter.Email,
                OpeningClosing = shelter.OpeningClosing,
                ShelterImage = shelter.ShelterImage,
                Description = shelter.Description
            };
        }
    }
}
